using MammutNg.MessageBrokers.Contracts;

namespace MammutNg.MessageBrokers.Nats
{
    public interface IBusSubscriber:IMessageSubscriber,IEventSubscriber,ICommandSubscriber,IQuerySubscriber
    {
        IBusSubscriber SubscribeMessage<T>() where T : class, IMessage;
        IBusSubscriber SubscribeQuery<TQuery, TResult>() where TQuery : class, IQuery<TResult>;
        IBusSubscriber SubscribeCommand<T>() where T : class, ICommand;
        IBusSubscriber SubscribeCommand<TCommand, TResult>() where TCommand : class, ICommand<TResult>;
        IBusSubscriber SubscribeEvent<T>() where T : class, IEvent;
     
    
    }
}