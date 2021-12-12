using System;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using MammutNg.Types;
using Microsoft.Extensions.DependencyInjection;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class MessageSubscriber:IMessageSubscriber
    {

        private readonly IStanConnection _connection;
        private readonly IServiceScopeFactory _rootServiceProvider;
        private readonly INatsMessageSerializer _messageSerializer;
        private readonly AppOptions _appOptions;

        public MessageSubscriber(IStanConnection connection, IServiceScopeFactory rootServiceProvider, INatsMessageSerializer messageSerializer, AppOptions appOptions)
        {
            _connection = connection;
            _rootServiceProvider = rootServiceProvider;
            _messageSerializer = messageSerializer;
            _appOptions = appOptions;
        }

  

        private Task SubscribeMessageAsync<T>(string topic,string queue) where T : class, IMessage
        {
            
            _connection.NATSConnection.SubscribeAsync(topic,queue, async(_, args) =>
            {
                try
                {
                    using var scope = _rootServiceProvider.CreateScope();
                    var context = _messageSerializer.Deserialize<MessageContext<T>>(args.Message.Data);
                    var messagePropertiesAccessor = scope.ServiceProvider.GetService<IMessagePropertiesAccessor>();
                    messagePropertiesAccessor.MessageProperties=context.Map();
                    var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();
                   await handler.HandleMessage(context.Message);
                }
                catch (Exception e)
                {
                    
                    //handle exception
                }
            });
           return Task.CompletedTask;
        }

        public Task SubscribeMessageAsync<T>() where T : class, IMessage
        {
            var topic = $"message.{typeof(T).FullName}";
            var queue = _appOptions.ServiceId;
            return SubscribeMessageAsync<T>(topic, queue);
        }
    }
}