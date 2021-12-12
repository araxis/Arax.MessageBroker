namespace MammutNg.MessageBrokers.Nats
{
    public interface INatsBusDispatcher:IBusDispatcher,IAdvancedEventDispatcher,IAdvancedCommandDispatcher,IAdvancedMessageDispatcher,IAdvancedQueryDispatcher
    {
      
    }
}