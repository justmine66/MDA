using Dapper;
using MDA.Common;
using MDA.Common.Extensions;
using MDA.Event.Abstractions;
using MDA.EventStore.MySql.Poes;
using MDA.EventStore.MySql.Port.Adapters.Input;
using MDA.EventStore.MySql.Port.Adapters.Output;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.EventStore.MySql
{
    public class MySqlDomainEventStore : IDomainEventStore
    {
        /// <summary>
        /// 分隔符，默认："_"。用于分隔表名中的各个分类名称。
        /// </summary>
        public static readonly string KeyDelimiter = ":";
        private const string TableName = "DomainEventStream";
        private const string InsertSql = "INSERT INTO {0}(`EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn`) VALUES(@EventId,@EventSequence,@EventBody,@AggregateRootId,@CommandId,@AggregateRootTypeName,@OccurredOn)";
        private const string SelectSql = "SELECT `EventId`,`EventSequence`,`EventBody`,`AggregateRootId`,`CommandId`,`AggregateRootTypeName`,`OccurredOn` FROM {0} WHERE {1}";

        private readonly ILogger<MySqlDomainEventStore> _logger;
        private readonly MySqlDomainEventStoreOptions _options;
        private readonly IEventSerializer _serializer;

        private string _versionIndexName;
        private string _commandIndexName;
        private int _tableCount;
        private int _appendEventStreamTimeoutInSeconds;

        public MySqlDomainEventStore(
            ILogger<MySqlDomainEventStore> logger,
            IOptions<MySqlDomainEventStoreOptions> options,
            IEventSerializer serializer)
        {
            _options = options.Value;

            Assert.NotNullOrEmpty(_options.ConnectionString, nameof(_options.ConnectionString));

            _logger = logger;
            _serializer = serializer;

            _versionIndexName = "IX_DomainEventStream_AggId_Version";
            _commandIndexName = "IX_DomainEventStream_AggId_CommandId";

            _tableCount = _options.AggregateRootShardTableCount <= 0 ? 5 : _options.AggregateRootShardTableCount;
            _appendEventStreamTimeoutInSeconds = _options.AppendEventStreamTimeoutInSeconds <= 0 ? 30 : _options.AppendEventStreamTimeoutInSeconds;
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
                        await connection.ExecuteAsync(sql, storedEvents, transaction, _appendEventStreamTimeoutInSeconds);
                        await transaction.CommitAsync();

                        return new AsyncResult<DomainEventAppendResult>(AsyncStatus.Success);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            await transaction.RollbackAsync();
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

        public async Task<AsyncResult<int>> CountStoredEventsAsync()
        {
            try
            {
                using (var connection = new MySqlConnection(_options.ConnectionString))
                {
                    await connection.OpenAsync();
                    var transaction = await connection.BeginTransactionAsync();

                    var count = 0;

                    try
                    {
                        for (int i = 0; i < _tableCount; i++)
                        {
                            var selectSql = $"SELECT count(1) FROM TableName_{i}";
                            count += await connection.ExecuteScalarAsync<int>(selectSql);
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            await transaction.RollbackAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"the method{nameof(CountStoredEventsAsync)}'s transaction rollback failed.", ex);
                        }

                        throw;
                    }

                    return new AsyncResult<int>(AsyncStatus.Success, count);
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError($"the {nameof(CountStoredEventsAsync)} has a sql excetiopn", ex);

                return new AsyncResult<int>(AsyncStatus.Failed);
            }
            catch (Exception ex)
            {
                _logger.LogError($"the {nameof(CountStoredEventsAsync)} has a unknown excetiopn", ex);

                return new AsyncResult<int>(AsyncStatus.Failed);
            }
        }

        public async Task<AsyncResult<IEnumerable<IDomainEvent>>> GetAllAggregateStoredEventsBetweenAsync(string aggregateRootId, string aggregateRootTypeName, int lowSequence, int highSequence)
        {
            Assert.NotNullOrEmpty(aggregateRootId, nameof(aggregateRootId));
            Assert.NotNullOrEmpty(aggregateRootTypeName, nameof(aggregateRootTypeName));

            var sql = string.Format(SelectSql, GetShardTableName(aggregateRootId), "`AggregateRootId`=@AggregateRootId AND `AggregateRootTypeName`=@AggregateRootTypeName AND `EventSequence`>=@LowSequence AND `EventSequence`<@HighSequence");

            try
            {
                using (var connection = new MySqlConnection(_options.ConnectionString))
                {
                    var storedEvents = await connection.QueryAsync<StoredDomainEvent>(sql, new
                    {
                        AggregateRootId = aggregateRootId,
                        AggregateRootTypeName = aggregateRootTypeName,
                        LowSequence = lowSequence,
                        HighSequence = highSequence
                    });

                    var events = storedEvents.Select(it => DomainEventAdapter.ToDomainEvent(it, _serializer));

                    return new AsyncResult<IEnumerable<IDomainEvent>>(AsyncStatus.Success, events);
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError($"the {nameof(GetAllAggregateStoredEventsBetweenAsync)} has a sql excetiopn, aggregateRootId: {aggregateRootId},aggregateRootTypeName: {aggregateRootTypeName},LowSequence: {lowSequence},HighSequence: {highSequence}", ex);

                return new AsyncResult<IEnumerable<IDomainEvent>>(AsyncStatus.Failed);
            }
            catch (Exception ex)
            {
                _logger.LogError($"the {nameof(GetAllAggregateStoredEventsBetweenAsync)} has a unknown excetiopn, aggregateRootId: {aggregateRootId},aggregateRootTypeName: {aggregateRootTypeName},LowSequence: {lowSequence},HighSequence: {highSequence}", ex);

                return new AsyncResult<IEnumerable<IDomainEvent>>(AsyncStatus.Failed);
            }
        }

        public async Task<AsyncResult<IEnumerable<IDomainEvent>>> GetAllAggregateStoredEventsSinceAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            int sequence)
        {
            Assert.NotNullOrEmpty(aggregateRootId, nameof(aggregateRootId));
            Assert.NotNullOrEmpty(aggregateRootTypeName, nameof(aggregateRootTypeName));

            var sql = string.Format(SelectSql, GetShardTableName(aggregateRootId), "`AggregateRootId`=@AggregateRootId AND `AggregateRootTypeName`=@AggregateRootTypeName AND `EventSequence`>@EventSequence");

            try
            {
                using (var connection = new MySqlConnection(_options.ConnectionString))
                {
                    var storedEvents = await connection.QueryAsync<StoredDomainEvent>(sql, new
                    {
                        AggregateRootId = aggregateRootId,
                        AggregateRootTypeName = aggregateRootTypeName,
                        EventSequence = sequence
                    });

                    var events = storedEvents.Select(it => DomainEventAdapter.ToDomainEvent(it, _serializer));

                    return new AsyncResult<IEnumerable<IDomainEvent>>(AsyncStatus.Success, events);
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError($"the {nameof(GetAllAggregateStoredEventsSinceAsync)} has a sql excetiopn, aggregateRootId: {aggregateRootId},aggregateRootTypeName: {aggregateRootTypeName},EventSequence: {sequence}", ex);

                return new AsyncResult<IEnumerable<IDomainEvent>>(AsyncStatus.Failed);
            }
            catch (Exception ex)
            {
                _logger.LogError($"the {nameof(GetAllAggregateStoredEventsSinceAsync)} has a unknown excetiopn, aggregateRootId: {aggregateRootId},aggregateRootTypeName: {aggregateRootTypeName},EventSequence: {sequence}", ex);

                return new AsyncResult<IEnumerable<IDomainEvent>>(AsyncStatus.Failed);
            }
        }

        private string GetShardTableName(string aggregateRootId)
        {
            if (_tableCount <= 1)
            {
                return TableName;
            }

            var index = HashCodeHelper.GetShardIndexOf(aggregateRootId, _tableCount);
            return $"{TableName}{KeyDelimiter}{index}";
        }
    }
}
