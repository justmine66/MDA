using Dapper;
using MDA.Common;
using MDA.Event.Abstractions;
using MDA.EventStore.MySql.Port.Adapters.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.EventStore.MySql
{
    public class MySqlEventStore : IDomainEventStore
    {
        private const string TableName = "DomainEventStream";
        private const string InsertSql = "INSERT INTO {0}(`EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn`) VALUES(@EventId,@EventSequence,@EventBody,@AggregateRootId,@CommandId,@AggregateRootTypeName,@OccurredOn)";
        private const string SelectSql = "SELECT `EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn` FROM {0}";

        private readonly ILogger<MySqlEventStore> _logger;
        private readonly MySqlEventStoreOptions _options;
        private readonly IEventSerializer _serializer;

        private string _versionIndexName;
        private string _commandIndexName;
        private int _tableCount;

        public MySqlEventStore(
            ILogger<MySqlEventStore> logger,
            IOptions<MySqlEventStoreOptions> options,
            IEventSerializer serializer)
        {
            _options = options.Value;

            Assert.NotNullOrEmpty(_options.ConnectionString, nameof(_options.ConnectionString));

            _logger = logger;
            _serializer = serializer;

            _versionIndexName = "IX_EventStream_AggId_Version";
            _commandIndexName = "IX_EventStream_AggId_CommandId";
            _tableCount = 5;
        }

        public async Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(DomainEventStream eventStream)
        {
            Assert.NotNull(eventStream, nameof(eventStream));
            Assert.NotNullOrEmpty(eventStream.AggregateRootId, nameof(eventStream.AggregateRootId));
            Assert.NotNullOrEmpty(eventStream.AggregateRootTypeName, nameof(eventStream.AggregateRootTypeName));
            Assert.LengthGreaterThan(nameof(eventStream.Events), eventStream.Events.Count(), 0);

            var sql = string.Format(InsertSql, GetShardTableName(eventStream.AggregateRootId));
            var storedEvents = eventStream.Events.Select(it => StoredDomainEventAdapter.ToStoredDomainEvent(it, _serializer));

            try
            {
                using (var connection = new MySqlConnection(_options.ConnectionString))
                {
                    await connection.OpenAsync();
                    var transaction = await connection.BeginTransactionAsync();

                    try
                    {
                        await connection.ExecuteAsync(sql, storedEvents, transaction);
                        transaction.Commit();

                        return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Success);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"the method{nameof(AppendAllAsync)}'s transaction rollback failed.", ex);
                        }

                        throw;
                    }
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Apend all domain events associated with a aggregation has sql exception.", ex);

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
                _logger.LogError("Apend all domain events associated with a aggregation has unknown exception.", ex);

                return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Failed);
            }
        }

        public async Task<AsyncResult<DomainEventAppendResult>> AppendAsync(IDomainEvent domainEvent)
        {
            Assert.NotNull(domainEvent, nameof(domainEvent));
            Assert.NotNullOrEmpty(domainEvent.Id, nameof(domainEvent.Id));
            Assert.NotNullOrEmpty(domainEvent.AggregateRootId, nameof(domainEvent.AggregateRootId));
            Assert.NotNullOrEmpty(domainEvent.AggregateRootTypeName, nameof(domainEvent.AggregateRootTypeName));
            Assert.NotNullOrEmpty(domainEvent.CommandId, nameof(domainEvent.CommandId));

            var storedEvent = StoredDomainEventAdapter.ToStoredDomainEvent(domainEvent, _serializer);
            var sql = string.Format(InsertSql, GetShardTableName(domainEvent.AggregateRootId));

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

        public Task<IDomainEvent[]> GetAllAggregateStoredEventsSinceAsync(string aggregateRootId, string aggregateRootTypeName, int sequence)
        {
            throw new System.NotImplementedException();
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
