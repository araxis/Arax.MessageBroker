using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MammutNg.MessageBrokers.Contracts;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public class QueryDispatcher: IAdvancedQueryDispatcher
    {
        private readonly IStanConnection _connection;
        private readonly INatsMessageSerializer _messageSerializer;

        public QueryDispatcher(IStanConnection connection, INatsMessageSerializer messageSerializer)
        {
            _connection = connection;
            _messageSerializer = messageSerializer;
        }

        private async Task<TResult> DispatchQueryAsync<TQuery, TResult>(string topic,MessageContext<TQuery> queryContext) where TQuery : class, IQuery<TResult>
        {
            var requestData = _messageSerializer.Serialize(queryContext);
            var rawResult = await _connection.NATSConnection.RequestAsync(topic, requestData);
            return _messageSerializer.Deserialize<TResult>(rawResult.Data);
           
        }

        public Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
        {
            var topic = $"query.{typeof(TQuery).FullName}";
            var context=new MessageContext<TQuery>
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                TraceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                Message = query,
                Headers = new Dictionary<string, object>()

            };

            return DispatchQueryAsync<TQuery, TResult>(topic, context);
        }

        public Task<TResult> DispatchQueryAsync<TQuery, TResult>(MessageContext<TQuery> context)
            where TQuery : class, IQuery<TResult>
        {
            var topic = $"query.{typeof(TQuery).FullName}";
            return DispatchQueryAsync<TQuery, TResult>(topic, context);
        }
    }
}