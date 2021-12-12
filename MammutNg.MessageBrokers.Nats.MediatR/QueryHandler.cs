using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using MediatR;

namespace MammutNg.MessageBrokers.Nats.MediatR
{
    public class QueryHandler<TQuery,TResult>:IQueryHandler<TQuery,TResult> where TQuery : class, IQuery<TResult>,IRequest<TResult>
    {
        private readonly IMediator _mediator;

        public QueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResult> Handle(TQuery query) => _mediator.Send(query);
    }
}