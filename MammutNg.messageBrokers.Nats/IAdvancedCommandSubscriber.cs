using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IAdvancedCommandSubscriber : ICommandSubscriber
    {
        Task SubscribeCommandAsync<T>(FetchConfig fetchConfig) where T : class, ICommand;
    }
}