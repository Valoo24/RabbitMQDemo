using RabbitMQDemo.Core.Abstracts;
using RabbitMQDemo.Sender.Models;
using RabbitMQDemo.Sender.Tools;

bool hasQuit = false;
IMessage message = new Message();
RabbitMQManager messageManager = new();

Console.WriteLine("Welcome to the RabbitMQ Demo by Benjamin Malemprée.");
Console.WriteLine("This window shows the sender client.");
Console.ReadKey();

do
{
    Console.Clear();
    Console.WriteLine("Please type the message to send then press \'Enter\'");
    string result = Console.ReadLine();
    if (string.IsNullOrEmpty(result))
        result = "This is a placeholder message, because user didn't type anything.";

    message.Content = result;

    try
    {
        messageManager.SendMessage(message);
        Console.WriteLine("The Message was sent succesfully.");
    }
    catch (Exception ex)
    {
        hasQuit = true;
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine("Please type \'r\' to send another message.");
    var keyResult = Console.ReadKey();
    if(keyResult.Key != ConsoleKey.R)
        hasQuit = true;

} while (!hasQuit);

Console.WriteLine("Goodbye !");
Console.ReadLine();