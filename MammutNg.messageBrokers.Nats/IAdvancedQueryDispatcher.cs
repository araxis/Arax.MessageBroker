using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IAdvancedQueryDispatcher : IQueryDispatcher
    {
        Task<TResult> DispatchQueryAsync<TQuery, TResult>(MessageContext<TQuery> context)
            where TQuery : class, IQuery<TResult>;
    }
}