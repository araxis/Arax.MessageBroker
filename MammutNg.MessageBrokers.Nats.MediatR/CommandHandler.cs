using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using MediatR;

namespace MammutNg.MessageBrokers.Nats.MediatR
{
    public class CommandHandler<TCommand>:ICommandHandler<TCommand> where TCommand : class, ICommand,IRequest
    {
        private readonly IMediator _mediator;

        public CommandHandler(IMediator mediator)
        {
            _mediator = mediator;
       
        }

        public Task Handle(TCommand command)
        {
           
            return _mediator.Send(command);
        }
    }

    public class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand<TResult>,IRequest<TResult>
    {

        private readonly IMediator _mediator;
        public CommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResult> Handle(TCommand command) => _mediator.Send(command);
    }


}