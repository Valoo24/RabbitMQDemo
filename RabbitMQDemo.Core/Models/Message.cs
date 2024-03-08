using RabbitMQDemo.Core.Abstracts;

namespace RabbitMQDemo.Core.Models;

public class Message : IMessage
{
    public string Content { get; set; } = string.Empty;
}