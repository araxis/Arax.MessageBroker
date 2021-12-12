using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IQueryHandler<in TQuery,TResult> where TQuery:class,IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}