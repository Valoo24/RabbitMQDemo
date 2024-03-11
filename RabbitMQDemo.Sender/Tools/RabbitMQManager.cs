using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Core.Repositories;
using RabbitMQDemo.Sender.Models;
using System.Text;

namespace RabbitMQDemo.Sender.Tools;

public class RabbitMQManager : IRabbitMQManager<Message>
{
    private readonly IModel channel;
    private EventingBasicConsumer consumer;
    public RabbitMQManager()
    {
        var connectionFactory = new ConnectionFactory()
        {
            Uri = new("amqp://guest:guest@localhost:5672")
        };
        var connection = connectionFactory.CreateConnection();

        channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "demoexchange", type: ExchangeType.Direct);
        channel.QueueDeclare(queue: "demoqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: "demoqueue", exchange: "demoexchange", routingKey: "");
        InitializeReader();
    }

    public void InitializeReader()
    {
        consumer = new EventingBasicConsumer(channel);
        consumer.Received += (chan, ea) =>
        {
            var body = ea.Body;
            var jsonResult = Encoding.UTF8.GetString(body.ToArray());
            if (jsonResult is not null)
            {
                IMessage message = JsonConvert.DeserializeObject<Message>(jsonResult);
                MessageContext.messages.Add(message);
            }
        };
        channel.BasicConsume(queue: "demoqueue", autoAck: true, consumer: consumer);
    }

    public void SendMessage(Message message)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var messageproperties = channel.CreateBasicProperties();
        messageproperties.ContentType = "application/json";

        try
        {
            channel.BasicPublish(exchange: "demoexchange", routingKey: "",
                basicProperties: messageproperties, body: Encoding.UTF8.GetBytes(jsonMessage));
        }
        catch
        {
            throw;
        }
    }

    public void ReadMessage()
    {
        try
        {
            channel.BasicConsume(queue: "demoqueue", autoAck: true, consumer: consumer);
        }
        catch
        {
            throw;
        }
    }

    public void Dispose()
    {
        if (!channel.IsClosed) channel.Close();
    }
}