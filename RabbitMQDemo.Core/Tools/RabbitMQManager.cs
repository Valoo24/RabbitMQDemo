using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Sender.Models;
using Newtonsoft.Json;
using RabbitMQDemo.Core.Repositories;
using System.Text;

namespace RabbitMQDemo.Core.Tools;

public class RabbitMQManager : IRabbitMQManager<Message>
{
    private readonly IModel channel;
    private EventingBasicConsumer consumer;
    private string ExchangeName = string.Empty;
    private string QueueName = string.Empty;
    private string Routing = string.Empty;
    public RabbitMQManager(string connectionString, string exchangeName, string queueName, string routingKey = null)
    {
        if (routingKey is null)
            Routing = string.Empty;

        ExchangeName = exchangeName;
        QueueName = queueName;

        var connectionFactory = new ConnectionFactory()
        {
            Uri = new(connectionString)
        };
        var connection = connectionFactory.CreateConnection();

        channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct);
        channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: Routing);
        InitializeReader();
    }
    public virtual void InitializeReader()
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
        channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
    }

    public virtual void ReadMessage()
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

    public virtual void SendMessage(Message message)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var messageproperties = channel.CreateBasicProperties();
        messageproperties.ContentType = "application/json";

        try
        {
            channel.BasicPublish(exchange: QueueName, routingKey: Routing,
                basicProperties: messageproperties, body: Encoding.UTF8.GetBytes(jsonMessage));
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