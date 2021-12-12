using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IEventHandler<in TEvent> where TEvent:class,IEvent
    {
        Task Handle(TEvent @event);

    }
}