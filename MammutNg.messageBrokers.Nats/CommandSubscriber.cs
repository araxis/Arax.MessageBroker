using System;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using Microsoft.Extensions.DependencyInjection;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class CommandSubscriber: IAdvancedCommandSubscriber
    {

        private readonly IStanConnection _connection;
        private readonly IServiceScopeFactory _rootServiceProvider;
        private readonly INatsMessageSerializer _messageSerializer;
        public CommandSubscriber(IStanConnection connection, IServiceScopeFactory rootServiceProvider, INatsMessageSerializer messageSerializer)
        {
            _connection = connection;
            _rootServiceProvider = rootServiceProvider;
            _messageSerializer = messageSerializer;

        }



        private  Task SubscribeCommandAsync<TCommand, TResult>(string topic,string queue) where TCommand : class, ICommand<TResult>
        {
     
            _connection.NATSConnection.SubscribeAsync(topic, queue, async (_, args) =>
            {
                try
                {
                    using var scope=_rootServiceProvider.CreateScope();
                    var context = _messageSerializer.Deserialize<MessageContext<TCommand>>(args.Message.Data);
                    var messagePropertiesAccessor = scope.ServiceProvider.GetService<IMessagePropertiesAccessor>();
                    messagePropertiesAccessor.MessageProperties=context.Map();
                    var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand,TResult>>();
                    var result= await  handler.Handle(context.Message);
                    var retData = _messageSerializer.Serialize(result);
                    _connection.NATSConnection.Publish(args.Message.Reply, retData);
                }
                catch (Exception e)
                {
                  //handle exception
                }
            });
            return Task.CompletedTask;
        }

        public Task SubscribeCommandAsync<T>(FetchConfig fetchConfig) where T : class, ICommand
        {
            
           
            
            var config = fetchConfig.SubscriptionOptions;
            config.ManualAcks = true;

            _connection.Subscribe(fetchConfig.Topic, fetchConfig.QueueName,config, async (_, args) =>
            {
                try
                {
                    using var scope = _rootServiceProvider.CreateScope();
                    var context = _messageSerializer.Deserialize<MessageContext<T>>(args.Message.Data);
                    var messagePropertiesAccessor = scope.ServiceProvider.GetService<IMessagePropertiesAccessor>();
                    messagePropertiesAccessor.MessageProperties=context.Map();
                    var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
                    await  handler.Handle(context.Message);
                }
                catch (Exception e)
                {
                    //handl exception
                }
                finally
                {
                    args.Message.Ack();
                }
            });
          
           
            return Task.CompletedTask;
        }

        public Task SubscribeCommandAsync<TCommand>() where TCommand : class, ICommand
        {
            var topic= $"command.{typeof(TCommand).FullName}";
            const string queue = "command_queue";
            var config=new ByDurable(topic,queue,"command_pointer");
            return SubscribeCommandAsync<TCommand>(config);
        }

        public Task SubscribeCommandAsync<TCommand, TResult>() where TCommand : class, ICommand<TResult>
        {
            var topic= $"command.{typeof(TCommand).FullName}";
            const string queue = "command_queue";
            return SubscribeCommandAsync<TCommand,TResult>(topic, queue);
        }
    }
}