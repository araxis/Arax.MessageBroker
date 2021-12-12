using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using Sharpnado.Tasks;

namespace MammutNg.MessageBrokers.Nats
{
    public class BusSubscriber : INatsBusSubscriber
    {

        private readonly IAdvancedCommandSubscriber _commandSubscriber;
        private readonly IAdvancedEventSubscriber _eventSubscriber;
        private readonly IMessageSubscriber _messageSubscriber;
        private readonly IQuerySubscriber _querySubscriber;

        public BusSubscriber(IAdvancedCommandSubscriber commandSubscriber, IAdvancedEventSubscriber eventSubscriber,
            IMessageSubscriber messageSubscriber, IQuerySubscriber querySubscriber)
        {
            _commandSubscriber = commandSubscriber;
            _eventSubscriber = eventSubscriber;
            _messageSubscriber = messageSubscriber;
            _querySubscriber = querySubscriber;
        }

        public IBusSubscriber SubscribeMessage<T>() where T : class, IMessage
        {
            TaskMonitor.Create(_messageSubscriber.SubscribeMessageAsync<T>());
            return this;
        }

        public IBusSubscriber SubscribeQuery<TQuery, TResult>() where TQuery : class, IQuery<TResult>
        {
            TaskMonitor.Create(_querySubscriber.SubscribeQueryAsync<TQuery,TResult>());
            return this;
        }

        public IBusSubscriber SubscribeCommand<T>() where T : class, ICommand
        {
            TaskMonitor.Create(_commandSubscriber.SubscribeCommandAsync<T>());
            return this;
        }

        public IBusSubscriber SubscribeCommand<TCommand, TResult>() where TCommand : class, ICommand<TResult>
        {
            TaskMonitor.Create(_commandSubscriber.SubscribeCommandAsync<TCommand,TResult>());
            return this;
        }

        public IBusSubscriber SubscribeEvent<T>() where T : class, IEvent
        {
            TaskMonitor.Create(_eventSubscriber.SubscribeEventAsync<T>());
            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(FetchConfig fetchConfig) where TEvent : class, IEvent
        {
            TaskMonitor.Create(_eventSubscriber.SubscribeEventAsync<TEvent>(fetchConfig));
            return this;
        }

        public IBusSubscriber SubscribeCommand<T>(FetchConfig fetchConfig) where T : class, ICommand
        {
            TaskMonitor.Create(_commandSubscriber.SubscribeCommandAsync<T>(fetchConfig));
            return this;
        }

        public Task SubscribeMessageAsync<T>() where T : class, IMessage
        {
           return _messageSubscriber.SubscribeMessageAsync<T>();
        }

        public Task SubscribeEventAsync<TEvent>() where TEvent : class, IEvent
        {
            return _eventSubscriber.SubscribeEventAsync<TEvent>();
        }

        public Task SubscribeCommandAsync<TCommand>() where TCommand : class, ICommand
        {
            return _commandSubscriber.SubscribeCommandAsync<TCommand>();
        }

        public Task SubscribeCommandAsync<TCommand, TResult>() where TCommand : class, ICommand<TResult>
        {
            return _commandSubscriber.SubscribeCommandAsync<TCommand, TResult>();
        }

        public Task SubscribeQueryAsync<TQuery, TResult>() where TQuery : class, IQuery<TResult>
        {
            return _querySubscriber.SubscribeQueryAsync<TQuery, TResult>();
        }
    }
}