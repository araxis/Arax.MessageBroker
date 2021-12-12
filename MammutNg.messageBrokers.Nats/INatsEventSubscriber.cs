using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IAdvancedEventSubscriber : IEventSubscriber
    {
        Task SubscribeEventAsync<TEvent>(FetchConfig fetchConfig) where TEvent : class, IEvent;
    }
}