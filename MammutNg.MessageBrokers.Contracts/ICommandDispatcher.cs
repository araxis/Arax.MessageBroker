using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface ICommandDispatcher
    {
        Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand:class,  ICommand;
        Task<TResult> DispatchCommandAsync<TCommand, TResult>(TCommand command) where TCommand :class, ICommand<TResult>   ;
    }
}