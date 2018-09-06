using MDA.Common;
using MDA.Event.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.EventStore.MySql
{
    public class MySqlEventStore : IDomainEventStore
    {
        private const string Table = "DomainEventStream";
        private const string InsertSql = "INSERT INTO {0}(`EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn`) VALUES(@EventId,@EventSequence,@EventBody,@AggregateRootId,@CommandId,@AggregateRootTypeName,@OccurredOn)";
        private const string SelectSql = "SELECT `EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn` FROM {0}";

        private readonly ILogger<MySqlEventStore> _logger;
        private readonly MySqlEventStoreOptions _options;
        private readonly IEventSerializer _serializer;

        public MySqlEventStore(
            ILogger<MySqlEventStore> logger, 
            IOptions<MySqlEventStoreOptions> options,
            IEventSerializer serializer)
        {
            _logger = logger;
            _options = options.Value;
            _serializer = serializer;
        }

        public Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(IEnumerable<IDomainEvent> eventStream)
        {
            throw new System.NotImplementedException();
        }

        public Task<AsyncResult<DomainEventAppendResult>> AppendAsync(IDomainEvent domainEvent)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> CountStoredEventsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IDomainEvent[]> GetAllAggregateStoredEventsBetweenAsync(string aggregateRootId, string aggregateRootTypeName, long lowSequence, long highSequence)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDomainEvent[]> GetAllAggregateStoredEventsBetweenAsync<TType>(TType aggregateRootId, string aggregateRootTypeName, long lowSequence, long highSequence)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDomainEvent[]> GetAllAggregateStoredEventsSinceAsync(string aggregateRootId, string aggregateRootTypeName, int sequence)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDomainEvent[]> GetAllAggregateStoredEventsSinceAsync<TType>(TType aggregateRootId, string aggregateRootTypeName, int sequence)
        {
            throw new System.NotImplementedException();
        }
    }
}
