using System;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public abstract class FetchConfig
    {
        protected FetchConfig(string topic, string queueName,string durableName)
        {
            Topic = topic;
            QueueName = queueName;
            DurableName = durableName;
        }

        public string DurableName { get; }

        public string Topic { get; }

        public string QueueName { get; }

        public StanSubscriptionOptions SubscriptionOptions => BuildConfig();

        protected abstract StanSubscriptionOptions  BuildConfig();
    }

    public class DefaultBuilder: FetchConfig
    {
        protected override StanSubscriptionOptions BuildConfig() => StanSubscriptionOptions.GetDefaultOptions();

        public DefaultBuilder(string topic, string queueName, string durableName) : base(topic, queueName, durableName)
        {
        }
    }

    public class ByDurable: FetchConfig
    {

 

        public ByDurable(string topic,string queue,string durableName) : base(topic,queue,durableName)
        {
            
        }

        protected override StanSubscriptionOptions BuildConfig()
        {
            var config= StanSubscriptionOptions.GetDefaultOptions();
            if(!string.IsNullOrWhiteSpace(DurableName)){config.DurableName = DurableName;}
            return config;
        }

      
    }

    public class DeliverAllAvailable : FetchConfig
    {
        public DeliverAllAvailable(string topic, string queueName, string durableName) : base(topic, queueName, durableName)
        {
        }

        protected override StanSubscriptionOptions BuildConfig()
        {
           var config= StanSubscriptionOptions.GetDefaultOptions();
           if(!string.IsNullOrWhiteSpace(DurableName)){config.DurableName = DurableName;}

            config.DeliverAllAvailable();
            return config;
        }

       
    }

    public class FromSequenceNumber : FetchConfig
    {

        public ulong SequenceNumber { get; }
    

        public FromSequenceNumber(ulong sequenceNumber,string topic, string queueName, string durableName) : base(topic,queueName, durableName)
        {
            SequenceNumber = sequenceNumber;
          
        }

        protected override StanSubscriptionOptions BuildConfig()
        {
            var config= StanSubscriptionOptions.GetDefaultOptions();
            if(!string.IsNullOrWhiteSpace(DurableName)){config.DurableName = DurableName;}
            config.StartAt(SequenceNumber);
            return config;
        }
    }

    public class FromTime : FetchConfig
    {
     

        public DateTime StartTime { get; }

        public FromTime( DateTime startTime,string topic, string queueName, string durableName) : base(topic, queueName, durableName)
        {
            StartTime = startTime;
        }

        protected override StanSubscriptionOptions BuildConfig()
        {
            var config= StanSubscriptionOptions.GetDefaultOptions();
            if(!string.IsNullOrWhiteSpace(DurableName)){config.DurableName = DurableName;}
            config.StartAt(StartTime);
            return config;
        }
    }

    public class FromPassTime : FetchConfig
    {
     

        public TimeSpan Span { get; }

        public FromPassTime( TimeSpan span,string topic, string queueName, string durableName) : base(topic, queueName, durableName)
        {
            Span = span;
        }

        protected override StanSubscriptionOptions BuildConfig()
        {
            var config= StanSubscriptionOptions.GetDefaultOptions();
            if(!string.IsNullOrWhiteSpace(DurableName)){config.DurableName = DurableName;}
            config.StartAt(Span);
            return config;
        }
    }


}