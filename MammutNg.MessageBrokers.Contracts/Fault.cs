using System;

namespace MammutNg.MessageBrokers.Contracts
{
    public class Fault<T>:IMessage 
    {
       public string FaultId { get; set; }
       public string FaultedMessageId { get; set; }
       public DateTime Timestamp { get; set; }
       public ExceptionInfo[] Exceptions { get; set; }
       public HostInfo Host { get; set; }
       public T Message { get; set; }
    }
}