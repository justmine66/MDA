using Dapper;
using MDA.Common;
using MDA.Event.Abstractions;
using MDA.EventStore.MySql.Port.Adapters.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
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

        private string _versionIndexName;
        private string _commandIndexName;

        public MySqlEventStore(
            ILogger<MySqlEventStore> logger,
            IOptions<MySqlEventStoreOptions> options,
            IEventSerializer serializer)
        {
            _logger = logger;
            _options = options.Value;
            _serializer = serializer;

            _versionIndexName = "IX_EventStream_AggId_Version";
            _commandIndexName = "IX_EventStream_AggId_CommandId";

            Assert.NotNullOrEmpty(_options.ConnectionString, nameof(_options.ConnectionString));
        }

        public Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(IEnumerable<IDomainEvent> eventStream)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AsyncResult<DomainEventAppendResult>> AppendAsync(IDomainEvent domainEvent)
        {
            Assert.NotNull(domainEvent, nameof(domainEvent));
            Assert.NotNullOrEmpty(domainEvent.Id, nameof(domainEvent.Id));
            Assert.NotNullOrEmpty(domainEvent.AggregateRootId, nameof(domainEvent.AggregateRootId));
            Assert.NotNullOrEmpty(domainEvent.AggregateRootTypeName, nameof(domainEvent.AggregateRootTypeName));
            Assert.NotNullOrEmpty(domainEvent.CommandId, nameof(domainEvent.CommandId));

            var storedEvent = StoredDomainEventAdapter.ToStoredDomainEvent(domainEvent, _serializer);
            var sql = string.Format(InsertSql, $"{Table}_{domainEvent.AggregateRootTypeName}");

            try
            {
                using (var connection = new MySqlConnection(_options.ConnectionString))
                {
                    await connection.ExecuteAsync(sql, storedEvent);
                    return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Success);
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Append domain event has sql exception, eventInfo: " + storedEvent, ex);

                if (ex.Number == 1062 && ex.Message.Contains(_versionIndexName))
                {
                    return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Success, DomainEventAppendResult.DuplicateEvent);
                }
                else if (ex.Number == 1062 && ex.Message.Contains(_commandIndexName))
                {
                    return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Success, DomainEventAppendResult.DuplicateCommand);
                }
                else
                {
                    return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Failed);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Append domain event has sql unknown, eventInfo: " + storedEvent, ex);

                return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Failed, ex.Message);
            }
        }

        public Task<int> CountStoredEventsAsync()
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
