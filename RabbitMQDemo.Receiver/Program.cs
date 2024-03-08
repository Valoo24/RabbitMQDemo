using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

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