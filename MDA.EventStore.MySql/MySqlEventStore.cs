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
        private const string Table = "EventStream";
        private const string InsertSql = "INSERT INTO {0}(TypeName,EventBody,OccurredOn) VALUES(@TypeName,@EventBody,@OccurredOn)";
        private const string SelectSql = "SELECT `EventId`,`TypeName`,`EventBody`,`OccurredOn` FROM {0}";

        private readonly ILogger<MySqlEventStore> _logger;
        private readonly MySqlEventStoreOptions _options;

        public MySqlEventStore(ILogger<MySqlEventStore> logger, IOptions<MySqlEventStoreOptions> options)
        {
            _logger = logger;
            _options = options.Value;
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
