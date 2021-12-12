using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IEventDispatcher
    {
        Task DispatchEventAsync<TEvent>(TEvent @event) where TEvent:class,IEvent;
    }
}