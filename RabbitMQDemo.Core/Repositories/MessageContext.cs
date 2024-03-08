using RabbitMQDemo.Core.Abstracts;

namespace RabbitMQDemo.Core.Repositories;

public static class MessageContext
{
    public static IList<IMessage> messages = new List<IMessage>();
}