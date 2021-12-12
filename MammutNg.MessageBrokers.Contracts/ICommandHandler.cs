using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface ICommandHandler<in TCommand> where TCommand:class,ICommand
    {
        Task Handle(TCommand command);

    }

    public interface ICommandHandler<in TCommand,TResult> where TCommand:class,ICommand<TResult>

    {
        Task<TResult> Handle(TCommand command);

    }
}