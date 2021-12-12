using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public class BusDispatcher:INatsBusDispatcher
    {
        private readonly IAdvancedMessageDispatcher _messageDispatcher;
        private readonly IAdvancedEventDispatcher _eventDispatcher;
        private readonly IAdvancedQueryDispatcher _queryDispatcher;
        private readonly IAdvancedCommandDispatcher _commandDispatcher;

        public BusDispatcher( IAdvancedEventDispatcher eventDispatcher,IAdvancedMessageDispatcher messageDispatcher,
            IAdvancedQueryDispatcher queryDispatcher, IAdvancedCommandDispatcher commandDispatcher)
        {
            _messageDispatcher = messageDispatcher;
            _eventDispatcher = eventDispatcher;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public Task DispatchMessageAsync<TMessage>(TMessage message) where TMessage : class, IMessage => _messageDispatcher.DispatchMessageAsync(message);
        public Task DispatchMessageAsync<TMessage>(MessageContext<TMessage> context) where TMessage : class, IMessage => _messageDispatcher.DispatchMessageAsync(context);
        public Task DispatchEventAsync<TEvent>(TEvent @event) where TEvent : class, IEvent => _eventDispatcher.DispatchEventAsync(@event);

        public Task DispatchEventAsync<TEvent>(MessageContext<TEvent> context) where TEvent : class, IEvent => _eventDispatcher.DispatchEventAsync(context);

        public Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : class, ICommand => _commandDispatcher.DispatchCommandAsync(command);

        public Task<TResult> DispatchCommandAsync<TCommand, TResult>(TCommand command) where TCommand : class, ICommand<TResult> => _commandDispatcher.DispatchCommandAsync<TCommand, TResult>(command);

        public Task DispatchCommandAsync<T>(MessageContext<T> commandContext) where T : class, ICommand => _commandDispatcher.DispatchCommandAsync(commandContext);

        public Task<TResult> DispatchCommandAsync<TCommand, TResult>(MessageContext<TCommand> context) where TCommand : class, ICommand<TResult> => _commandDispatcher.DispatchCommandAsync<TCommand, TResult>( context);

        public Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult> => _queryDispatcher.DispatchQueryAsync<TQuery, TResult>(query);

        public Task<TResult> DispatchQueryAsync<TQuery, TResult>(MessageContext<TQuery> queryContext) where TQuery : class, IQuery<TResult> => _queryDispatcher.DispatchQueryAsync<TQuery, TResult>(queryContext);
    }
}