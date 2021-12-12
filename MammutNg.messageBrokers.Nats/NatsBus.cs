using Microsoft.Extensions.DependencyInjection;

namespace MammutNg.MessageBrokers.Nats
{
    public class NatsBus
    {
        public NatsBus(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public IServiceCollection ServiceCollection { get; }

    }
}