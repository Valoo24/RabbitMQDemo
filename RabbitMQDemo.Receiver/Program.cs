using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;


Console.WriteLine("Options (type the number corresponding of what you want to do)");
Console.WriteLine("1 - Send a message\t2 - Read all messages in queue");

var keyResult = Console.ReadKey();

switch(keyResult.Key)
{
    case ConsoleKey.D1:
        sendMessage();
        break;
    case ConsoleKey.D2:
        readMessages();
        break;
    default:
        Console.WriteLine("The key pressed, is not recognised.");
        break;
}

void readMessages()
{
    bool hasQuit = false;
    var factory = new ConnectionFactory()
    {
        Uri = new("amqp://guest:guest@localhost:5672")
    };

    do
    {
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "demoqueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (chan, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Message reçu : {message}");
            };

            channel.BasicConsume(queue: "demoqueue", autoAck: true, consumer: consumer);

            Console.WriteLine("Press \'q\' to quit, or any key to reload the page.\nWatch out, all messages will be lost !");
            var result = Console.ReadKey();
            if (result.Key == ConsoleKey.Q)
                hasQuit = true;
        }
    }
    while (!hasQuit);
}

void sendMessage()
{
    bool hasQuit = false;
}