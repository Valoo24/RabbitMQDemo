using RabbitMQDemo.Sender.Models;
using RabbitMQDemo.Sender.Tools;
using RabbitMQDemo.Core.Repositories;

RabbitMQManager messageManager = new();

do
{
    Console.Clear();
    Console.WriteLine("Options (type the number corresponding of what you want to do) :");
    Console.WriteLine("1 - Send a message\t2 - Read all messages in queue");

    var keyResult = Console.ReadKey(true);
    switch (keyResult.Key)
    {
        case ConsoleKey.NumPad1:
            sendMessage();
            break;
        case ConsoleKey.NumPad2:
            readMessages();
            break;
        default:
            Console.WriteLine("The key pressed, is not recognised.");
            break;
    }
}
while (!quitPrompt());

Console.WriteLine("Goodbye !");
Console.ReadLine();

void readMessages()
{
    Console.Clear();
    try
    {
        messageManager.ReadMessage();

        Console.WriteLine("Messages :");
        foreach (var message in MessageContext.messages)
        {
            if(message is not null)
            Console.WriteLine($"Message : {message.Content}");
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void sendMessage()
{
    Message message = new Message();

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
        Console.WriteLine(ex.Message);
    }
}

bool quitPrompt()
{
    Console.WriteLine("Press \'q\' to quit, or any key to reload.");
    if (Console.ReadKey(true).Key == ConsoleKey.Q) return true;
    else
    {
        return false;
    }
}