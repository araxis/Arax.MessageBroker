using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IBusDispatcher:IMessageDispatcher,IEventDispatcher,ICommandDispatcher,IQueryDispatcher
    {


    }
}