using RabbitMQDemo.Core.Abstracts;

namespace RabbitMQDemo.Sender.Models;

public class Message : IMessage
{
    public string Content { get; set; } = string.Empty;
}