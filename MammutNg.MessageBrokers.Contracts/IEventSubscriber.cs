using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IEventSubscriber
    {
        Task SubscribeEventAsync<TEvent>() where TEvent:class,IEvent;
    }
}