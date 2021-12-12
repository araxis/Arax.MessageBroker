using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class EventDispatcher: IAdvancedEventDispatcher
    {

        private readonly IStanConnection _connection;
        private readonly INatsMessageSerializer _messageSerializer;

        public EventDispatcher(IStanConnection connection, INatsMessageSerializer messageSerializer)
        {
            _connection = connection;
            _messageSerializer = messageSerializer;
        }
        public Task DispatchEventAsync<TEvent>(string topic,MessageContext<TEvent> context) where TEvent : class, IEvent
        {
          
           
            var data = _messageSerializer.Serialize(context);
            return _connection.PublishAsync(topic, data);

        }
        public Task DispatchEventAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var topic = $"event.{typeof(TEvent).FullName}";
            var context=new MessageContext<TEvent>
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                TraceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                Message = @event,
                Headers = new Dictionary<string, object>()

            };
            return DispatchEventAsync(topic, context);
        }

        public Task DispatchEventAsync<TEvent>(MessageContext<TEvent> context) where TEvent : class, IEvent
        {
            var topic = $"event.{typeof(TEvent).FullName}";
            return DispatchEventAsync(topic, context);
        }
    }
}