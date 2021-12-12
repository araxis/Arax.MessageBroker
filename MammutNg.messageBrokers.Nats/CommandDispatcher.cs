using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class CommandDispatcher: IAdvancedCommandDispatcher
    {
        private readonly IStanConnection _connection;
        private readonly INatsMessageSerializer _messageSerializer;

        public CommandDispatcher(IStanConnection connection, INatsMessageSerializer messageSerializer)
        {
            _connection = connection;
            _messageSerializer = messageSerializer;
        }

        public Task DispatchCommandAsync<T>(string topic,MessageContext<T> commandContext) where T : class, ICommand
        {
            var data = _messageSerializer.Serialize(commandContext);
            return _connection.PublishAsync(topic, data);
        }

        public async Task<TResult> DispatchCommandAsync<TCommand, TResult>(string topic,MessageContext<TCommand> context) where TCommand : class, ICommand<TResult>
        {
           
            var requestData = _messageSerializer.Serialize(context);
            var rawResult = await _connection.NATSConnection.RequestAsync(topic, requestData);
            var result = _messageSerializer.Deserialize<TResult>(rawResult.Data);
            return result;
        }

        public Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var topic= $"command.{typeof(TCommand).FullName}";
            var context=new MessageContext<TCommand>
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                TraceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                Message = command,
                Headers = new Dictionary<string, object>()

            };
            return DispatchCommandAsync<TCommand>(topic, context);
        }

        public Task<TResult> DispatchCommandAsync<TCommand, TResult>(TCommand command) where TCommand : class, ICommand<TResult>
        {
            var topic= $"command.{typeof(TCommand).FullName}";
            var context=new MessageContext<TCommand>
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                TraceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                Message = command,
                Headers = new Dictionary<string, object>()

            };
            return DispatchCommandAsync<TCommand, TResult>(topic,context);
        }

        public Task DispatchCommandAsync<T>(MessageContext<T> context) where T : class, ICommand
        {
            var topic= $"command.{typeof(T).FullName}";
            return DispatchCommandAsync(topic, context);
        }

        public Task<TResult> DispatchCommandAsync<TCommand, TResult>(MessageContext<TCommand> context)
            where TCommand : class, ICommand<TResult>
        {
            var topic= $"command.{typeof(TCommand).FullName}";
            return DispatchCommandAsync<TCommand, TResult>(topic,context);
        }
    }
}