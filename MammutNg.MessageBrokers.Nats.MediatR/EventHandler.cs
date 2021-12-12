using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using MediatR;

namespace MammutNg.MessageBrokers.Nats.MediatR
{
    public class EventHandler<TEvent>:IEventHandler<TEvent> where TEvent : class, IEvent,INotification
    {
        private readonly IMediator _mediator;

        public EventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(TEvent @event) => _mediator.Publish(@event);
    }
}