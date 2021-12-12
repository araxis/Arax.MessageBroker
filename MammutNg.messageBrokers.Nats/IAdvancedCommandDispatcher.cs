using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IAdvancedCommandDispatcher : ICommandDispatcher
    {
        Task DispatchCommandAsync<T>(MessageContext<T> context) where T : class, ICommand;

        Task<TResult> DispatchCommandAsync<TCommand, TResult>(MessageContext<TCommand> context)
            where TCommand : class, ICommand<TResult>;
    }
}