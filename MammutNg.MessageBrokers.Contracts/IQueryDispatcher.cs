using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query) where TQuery:class,IQuery<TResult>;
    }
}