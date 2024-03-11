namespace RabbitMQDemo.Core.Abstracts;

public interface IRabbitMQManager<TMessageObject> : IDisposable
{
    public void InitializeReader();
    public void SendMessage(TMessageObject message);
    public void ReadMessage();
}