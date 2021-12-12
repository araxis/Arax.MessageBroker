using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IMessageDispatcher
    {
        
        Task DispatchMessageAsync<TMessage>(TMessage message) where TMessage :class, IMessage;
    }
}
