using System.Threading.Tasks;

namespace MammutNg.MessageBrokers.Contracts
{
    public interface IFaultSubscriber
    {
        Task SubscribeFaultAsync<T>() where T : class;
    }
}