using System.Text.Json;

namespace MammutNg.MessageBrokers.Nats
{
    public class MessageSerializer : INatsMessageSerializer
    {
        public byte[] Serialize<T>(T message) 
        {
            return JsonSerializer.SerializeToUtf8Bytes(message);
        }

        public T Deserialize<T>(byte[] data) 
        {
            
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}