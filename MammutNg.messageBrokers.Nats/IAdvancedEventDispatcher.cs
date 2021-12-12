using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IAdvancedEventDispatcher : IEventDispatcher
    {
        Task DispatchEventAsync<TEvent>(MessageContext<TEvent> context) where TEvent : class, IEvent;
    }
}