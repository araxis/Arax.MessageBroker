using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IQuerySubscriber
    {
        Task SubscribeQueryAsync<TQuery,TResult>() where TQuery:class,IQuery<TResult> ;
    }
}