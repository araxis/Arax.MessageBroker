using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface ICommandSubscriber
    {
        Task SubscribeCommandAsync<TCommand>() where TCommand:class,ICommand;
        Task SubscribeCommandAsync<TCommand,TResult>() where TCommand:class,ICommand<TResult>;
    }
}