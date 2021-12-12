using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface INatsBusSubscriber : IBusSubscriber
    {
        IBusSubscriber SubscribeEvent<TEvent>(FetchConfig fetchConfig) where TEvent : class, IEvent;
        IBusSubscriber SubscribeCommand<T>(FetchConfig fetchConfig) where T : class, ICommand;
    }
}