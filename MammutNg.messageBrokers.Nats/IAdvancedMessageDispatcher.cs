using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IAdvancedMessageDispatcher : IMessageDispatcher
    {
        Task DispatchMessageAsync<TMessage>(MessageContext<TMessage> context) where TMessage : class, IMessage;
    }
}