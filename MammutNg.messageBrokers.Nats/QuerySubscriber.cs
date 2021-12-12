using System;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class QuerySubscriber:IQuerySubscriber
    {

        private readonly IStanConnection _connection;
        private readonly IServiceScopeFactory _rootServiceProvider;
        private readonly INatsMessageSerializer _messageSerializer;



        public QuerySubscriber(IStanConnection connection, IServiceScopeFactory rootServiceProvider,
            INatsMessageSerializer messageSerializer)
        {
            _connection = connection;
            _rootServiceProvider = rootServiceProvider;
            _messageSerializer = messageSerializer;
        }

        private Task SubscribeQueryAsync<TQuery, TResult>(string topic,string queue) where TQuery : class, IQuery<TResult>
        {
            async void Handler(object _, MsgHandlerEventArgs args)
            {
                try
                {
                    using var scope = _rootServiceProvider.CreateScope();
                    var context = _messageSerializer.Deserialize<MessageContext<TQuery>>(args.Message.Data);
                    var messagePropertiesAccessor = scope.ServiceProvider.GetService<IMessagePropertiesAccessor>();
                    messagePropertiesAccessor.MessageProperties = context.Map();
                    var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
                    var result = await handler.Handle(context.Message);
                    var retData = _messageSerializer.Serialize(result);
                    _connection.NATSConnection.Publish(args.Message.Reply, retData);
                }
                catch (Exception e)
                {
                    //handle exception
                }
            }

            _connection.NATSConnection.SubscribeAsync(topic, queue, Handler);
            return Task.CompletedTask;
        }

        public Task SubscribeQueryAsync<TQuery, TResult>() where TQuery : class, IQuery<TResult>
        {
            var topic = $"query.{typeof(TQuery).FullName}";
            var queue = "query_queue";
            return SubscribeQueryAsync<TQuery, TResult>(topic, queue);
        }
    }
}