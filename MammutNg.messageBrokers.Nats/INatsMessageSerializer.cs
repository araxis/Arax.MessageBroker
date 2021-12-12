namespace MammutNg.MessageBrokers.Nats
{
    public interface INatsMessageSerializer
    { 
        byte[] Serialize<T>(T message) ;
        T Deserialize<T>(byte[] data);
    }
}