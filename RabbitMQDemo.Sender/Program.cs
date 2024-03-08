using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Core.Models;
using RabbitMQDemo.Core.Tools;

IMessage message = new Message();
RabbitMQManager messageManager = new();

Console.WriteLine("Welcome to the RabbitMQ Demo by Benjamin Malemprée.");
Console.WriteLine("This window shows the sender client.");
Console.ReadLine();

Console.Clear();
Console.WriteLine("Please type the message to send then press \'Enter\'");
string result = Console.ReadLine();
if (string.IsNullOrEmpty(result))
    result = "This is a placeholder message, because user didn't type anything.";

message.Content = result;

Console.Clear();
try
{
    messageManager.SendMessage(message);
    Console.WriteLine("The Message was sent succesfully.");
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.ReadLine();