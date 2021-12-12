namespace MammutNg.MessageBrokers.Contracts
{
    public interface IMessagePropertiesAccessor
    {
        IMessageProperties MessageProperties { get; set; }
    }
}