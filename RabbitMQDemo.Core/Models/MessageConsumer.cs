using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Core.Tools;
using System.Text;

namespace RabbitMQDemo.Core.Models;

public class MessageConsumer : DefaultBasicConsumer
{
    private readonly RabbitMQManager _manager;
    public MessageConsumer(RabbitMQManager manager)
    {
        _manager = manager;
    }

    public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
    {
        if (properties.ContentType != "application/json")
            throw new ArgumentException($"Can't handle content type {properties.ContentType}");

        var message = Encoding.UTF8.GetString(body.Span);
        var commandObj = JsonConvert.DeserializeObject<Message>(message);

        if (commandObj is null)
            throw new ArgumentNullException("The message received was null");

        Consume(commandObj);
        _manager.SendAck(deliveryTag);
    }

    private void Consume(IMessage message)
    {
        //Do Nothing
    }
}