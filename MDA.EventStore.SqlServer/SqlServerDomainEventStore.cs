using MDA.Common;
using MDA.Event.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.EventStore.SqlServer
{
    public class SqlServerDomainEventStore : IDomainEventStore
    {
        private const string TableName = "DomainEventStream";
        private const string InsertSql = "INSERT INTO {0}(`EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn`) VALUES(@EventId,@EventSequence,@EventBody,@AggregateRootId,@CommandId,@AggregateRootTypeName,@OccurredOn)";
        private const string SelectSql = "SELECT `EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn` FROM {0} WHERE {1}";

        private readonly ILogger<SqlServerDomainEventStore> _logger;
        private readonly SqlServerDomainEventStoreOptions _options;
        private readonly IEventSerializer _serializer;

        private string _versionIndexName;
        private string _commandIndexName;
        private int _tableCount;

        public SqlServerDomainEventStore(
            ILogger<SqlServerDomainEventStore> logger,
            IOptions<SqlServerDomainEventStoreOptions> options,
            IEventSerializer serializer)
        {
            _options = options.Value;

            Assert.NotNullOrEmpty(_options.ConnectionString, nameof(_options.ConnectionString));

            _logger = logger;
            _serializer = serializer;

            _versionIndexName = "IX_EventStream_AggId_Version";
            _commandIndexName = "IX_EventStream_AggId_CommandId";

            _tableCount = _options.AggregateRootShardTableCount <= 0 ? 5 : _options.AggregateRootShardTableCount;
        }

        public Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(DomainEventStream eventStream)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncResult<DomainEventAppendResult>> AppendAsync(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncResult<int>> CountStoredEventsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AsyncResult<IEnumerable<IDomainEvent>>> GetAllAggregateStoredEventsBetweenAsync(string aggregateRootId, string aggregateRootTypeName, int lowSequence, int highSequence)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncResult<IEnumerable<IDomainEvent>>> GetAllAggregateStoredEventsSinceAsync(string aggregateRootId, string aggregateRootTypeName, int sequence)
        {
            throw new NotImplementedException();
        }

        private string GetShardTableName(string aggregateRootId)
        {
            if (_tableCount <= 1)
            {
                return TableName;
            }

            var index = HashCodeHelper.GetShardIndexOf(aggregateRootId, _tableCount);
            return $"{TableName}_{index}";
        }
    }
}
