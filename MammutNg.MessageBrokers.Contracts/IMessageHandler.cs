using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IMessageHandler<in TMessage>where TMessage:class,IMessage
    {
        Task HandleMessage(TMessage message);
    }
}