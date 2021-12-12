using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using MediatR;

namespace MammutNg.MessageBrokers.Nats.MediatR
{
    public class MessageHandler<TMessage>:IMessageHandler<TMessage> where TMessage:class, IMessage,INotification
    {
        private readonly IMediator _mediator;

        public MessageHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

   

        public Task HandleMessage(TMessage message) => _mediator.Publish(message);
    }
}