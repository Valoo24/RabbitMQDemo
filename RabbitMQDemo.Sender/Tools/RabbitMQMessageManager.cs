using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Core.Repositories;
using RabbitMQDemo.Core.Tools;
using RabbitMQDemo.Sender.Models;
using System.Text;

namespace RabbitMQDemo.Sender.Tools;

public class RabbitMQMessageManager : RabbitMQManager
{
    public RabbitMQMessageManager(string exchangeName, string queueName) : base("amqp://guest:guest@localhost:5672", exchangeName, queueName) { }
}