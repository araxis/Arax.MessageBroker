using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IMessageSubscriber
    {
        Task SubscribeMessageAsync<T>() where T:class,IMessage;
    }
}