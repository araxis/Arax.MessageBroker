using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class MessageDispatcher: IAdvancedMessageDispatcher
    {
        private readonly IStanConnection _connection;
        private readonly INatsMessageSerializer _messageSerializer;

        public MessageDispatcher(IStanConnection connection, INatsMessageSerializer messageSerializer)
        {
            _connection = connection;
            _messageSerializer = messageSerializer;
        }

        public Task DispatchMessageAsync<TMessage>(string topic ,MessageContext<TMessage> context) where TMessage : class, IMessage
        {
            
            var data = _messageSerializer.Serialize(context);
            _connection.NATSConnection.Publish(topic, data);
            return Task.CompletedTask;
        }

        public Task DispatchMessageAsync<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            var topic = $"message.{typeof(TMessage).FullName}";
            var context=new MessageContext<TMessage>
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                TraceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                Message = message,
                Headers = new Dictionary<string, object>()

            };

            return DispatchMessageAsync(topic, context);
        }

        public Task DispatchMessageAsync<TMessage>(MessageContext<TMessage> context) where TMessage : class, IMessage
        {
            var topic = $"message.{typeof(TMessage).FullName}";
            return DispatchMessageAsync(topic, context);
        }
    }
}