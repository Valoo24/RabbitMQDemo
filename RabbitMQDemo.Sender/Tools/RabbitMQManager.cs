using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Sender.Models;
using System.Text;

namespace RabbitMQDemo.Sender.Tools;

public class RabbitMQManager : IDisposable
{
    private readonly IModel channel;
    public RabbitMQManager()
    {
        var connectionFactory = new ConnectionFactory()
        {
            Uri = new("amqp://guest:guest@localhost:5672")
        };
        var connection = connectionFactory.CreateConnection();

        channel = connection.CreateModel();
    }

    public void SendMessage(IMessage message)
    {

        channel.ExchangeDeclare(exchange: "demoexchange", type: ExchangeType.Direct);
        channel.QueueDeclare(queue: "demoqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: "demoqueue", exchange: "demoexchange", routingKey: "");

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

    public void ListenMessage()
    {
        channel.QueueDeclare(
            queue:"demoqueue", durable:true, exclusive:false, autoDelete:false, arguments:null);

        channel.BasicQos(prefetchSize: 0,prefetchCount:1,global:false);

        MessageConsumer consumer = new(this);

        channel.BasicConsume(queue: "demoqueue", false, consumer: consumer);
    }

    public void SendAck(ulong deliveryTag)
    {
        channel.BasicAck(deliveryTag:deliveryTag, multiple:false);
    }

    public void Dispose()
    {
        if (!channel.IsClosed) channel.Close();
    }
}