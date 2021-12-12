using System;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using MammutNg.Types;
using Microsoft.Extensions.DependencyInjection;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class EventSubscriber: IAdvancedEventSubscriber
    {
        private readonly IStanConnection _connection;
        private readonly IServiceScopeFactory _rootServiceProvider;
        private readonly INatsMessageSerializer _messageSerializer;
        private readonly AppOptions _appOptions;

        public EventSubscriber(IStanConnection connection, IServiceScopeFactory rootServiceProvider, INatsMessageSerializer messageSerializer, AppOptions appOptions)
        {
            _connection = connection;
            _rootServiceProvider = rootServiceProvider;
            _messageSerializer = messageSerializer;
            _appOptions = appOptions;
        }



        public Task SubscribeEventAsync<TEvent>(FetchConfig fetchConfig) where TEvent : class, IEvent
        {

            var config = fetchConfig.SubscriptionOptions;
            config.ManualAcks = true;
            _connection.Subscribe(fetchConfig.Topic,fetchConfig.QueueName,config,
                async (_, args) =>
                {
                    try
                    {
                        using var scope = _rootServiceProvider.CreateScope();
                        var context = _messageSerializer.Deserialize<MessageContext<TEvent>>(args.Message.Data);
                        var messagePropertiesAccessor = scope.ServiceProvider.GetService<IMessagePropertiesAccessor>();
                        messagePropertiesAccessor.MessageProperties=context.Map();
                        var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                        await handler.Handle(context.Message);

                    }
                    catch (Exception e)
                    {
                        // PublishError<T>(message, e);
                    }
                    finally
                    {
                        args.Message.Ack();
                    }
                   
                });
            return Task.CompletedTask;
        }

        public Task SubscribeEventAsync<TEvent>() where TEvent : class, IEvent
        {
            var topic = $"event.{typeof(TEvent).FullName}";
            var serviceId = _appOptions.ServiceId;
            var config=new ByDurable(topic,serviceId,serviceId);
            return SubscribeEventAsync<TEvent>(config);
        }
    }
}