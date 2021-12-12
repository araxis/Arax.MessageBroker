using System;
using System.Collections.Generic;

namespace MammutNg.MessageBrokers.Nats
{
    public class MessageContext<T> where T:class
    {
        public MessageContext(string messageId, string correlationId, string traceId, long timestamp, T message)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            TraceId = traceId;
            Timestamp = timestamp;
            Message = message;
            Headers=new Dictionary<string, object>();
        }

        public MessageContext()
        {
            MessageId = Guid.NewGuid().ToString();
            CorrelationId = Guid.NewGuid().ToString();
            TraceId = Guid.NewGuid().ToString();
            
            Headers=new Dictionary<string, object>(); 
        }
        public string MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string TraceId { get; set; }
        public long Timestamp { get; set; }
        public Dictionary<string,object> Headers { get; set; }

        public T Message { get; set; }
    }
}