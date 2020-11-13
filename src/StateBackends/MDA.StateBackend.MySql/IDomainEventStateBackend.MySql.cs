using MDA.Domain.Events;
using MDA.Infrastructure.Async;
using MDA.Infrastructure.Serialization;
using MDA.Infrastructure.Typing;
using MDA.Infrastructure.Utils;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.MySql
{
    public class MySqlDomainEventStateBackend : IDomainEventStateBackend
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;
        private readonly IDomainEventPublisher _eventPublisher;
        private readonly IBinarySerializer _binarySerializer;
        private readonly ITypeResolver _typeResolver;
        private readonly MySqlStateBackendOptions _options;

        public MySqlDomainEventStateBackend(
            IRelationalDbStorageFactory factory,
            ILogger<MySqlDomainEventStateBackend> logger,
            ITypeResolver typeResolver,
            IOptions<MySqlStateBackendOptions> options,
            IBinarySerializer binarySerializer,
            IDomainEventPublisher eventPublisher)
        {
            _logger = logger;
            _db = factory.CreateRelationalDbStorage(DatabaseScheme.StateDb);
            _typeResolver = typeResolver;
            _binarySerializer = binarySerializer;
            _eventPublisher = eventPublisher;
            _options = options.Value;
        }

        public async Task<DomainEventResult> AppendAsync(
            IDomainEvent @event,
            CancellationToken token = default)
        {
            if (!@event.IsValid())
            {
                return DomainEventResult.StorageFailed(@event.Id, $"The domain event: [{@event.Print()}] cannot be stored mysql state backend.");
            }

            var domainEventRecord = DomainEventRecordPortAdapter.ToDomainEventRecord(@event, _binarySerializer);
            var parameters = DbParameterProvider.ReflectionParameters(domainEventRecord);

            var tables = _options.DomainEventOptions.Tables;
            var insertDomainEventIndexSql = $"INSERT INTO `{tables.DomainEventIndices}`(`DomainCommandId`,`DomainCommandType`,`DomainCommandVersion`,`AggregateRootId`,`AggregateRootType`,`AggregateRootVersion`,`AggregateRootGeneration`,`DomainEventId`,`DomainEventType`,`DomainEventVersion`,`DomainEventPayloadBytes`,`CreatedTimestamp`) VALUES(@DomainCommandId,@DomainCommandType,@DomainCommandVersion,@AggregateRootId,@AggregateRootType,@AggregateRootVersion,@AggregateRootGeneration,@DomainEventId,@DomainEventType,@DomainEventVersion,@DomainEventPayloadBytes,@CreatedTimestamp)";
            var insertDomainEventSql = $"INSERT INTO `{tables.DomainEvents}`(`DomainEventId`,`Payload`) VALUES (@DomainEventId,@Payload)";

            var maxNumErrorTries = 3;
            var maxExecutionTime = TimeSpan.FromSeconds(3);
            var expectedRows = 2;
            bool ErrorFilter(Exception exc, int attempt)
            {
                if (exc is MySqlException inner &&
                    inner.HasDuplicateEntry())
                {
                    _logger.LogWarning($"{domainEventRecord.AggregateRootType}: [Ignored]find duplicated domain event from mysql state backend：{inner.Message}.");

                    return false;
                }

                _logger.LogError($"Append domain event has unknown exception: {LogFormatter.PrintException(exc)}.");

                return true;
            }

            var affectedRows = await AsyncExecutorWithRetries.ExecuteWithRetriesAsync(async attempt =>
            {
                return await _db.ExecuteAsync(
                    $"{insertDomainEventIndexSql};{insertDomainEventSql};",
                    command => command.Parameters.AddRange(parameters),
                    token);
            }, maxNumErrorTries, ErrorFilter, maxExecutionTime).ConfigureAwait(false);

            if (affectedRows != expectedRows)
                return DomainEventResult.StorageFailed(@event.Id,
                    $"The affected rows returned MySql state backend is incorrect, expected: {expectedRows}, actual: {affectedRows}.");

            await _eventPublisher.PublishAsync(@event, token);

            return DomainEventResult.StorageSucceed(@event.Id);
        }

        public async Task<IEnumerable<DomainEventResult>> AppendAsync(
            IEnumerable<IDomainEvent> events,
            CancellationToken token = default)
        {
            if (events.IsEmpty())
                return Enumerable.Empty<DomainEventResult>();

            var results = new List<DomainEventResult>();

            foreach (var @event in events)
            {
                var result = await AppendAsync(@event, token);

                results.Add(result);
            }

            return await Task.FromResult(results);
        }

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            long startOffset = 0,
            CancellationToken token = default)
            => await GetEventStreamAsync(
                aggregateRootId,
                startOffset,
                long.MaxValue,
                token);

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            long startOffset = 0,
            long endOffset = long.MaxValue,
            CancellationToken token = default)
        {
            PreConditions.NotNullOrEmpty(aggregateRootId, nameof(aggregateRootId));
            PreConditions.Nonnegative(startOffset, nameof(startOffset));

            var tables = _options.DomainEventOptions.Tables;

            var sql = $"SELECT d.`DomainCommandId`,d.`DomainCommandType`,d.`DomainCommandVersion`,d.`AggregateRootId`,d.`AggregateRootType`,d.`AggregateRootVersion`,d.`AggregateRootGeneration`,d.`DomainEventId`,d.`DomainEventType`,d.`DomainEventVersion`,d.`CreatedTimestamp`, p.`Payload` FROM `{tables.DomainEventIndices}` d INNER JOIN `{tables.DomainEvents}` p ON d.`DomainEventId`=p.`DomainEventId` WHERE d.`AggregateRootId`=@AggregateRootId AND d.AggregateRootVersion>=@StartOffset AND d.AggregateRootVersion<@EndOffset";

            var records = await _db.ReadAsync<DomainEventRecord>(sql, new
            {
                AggregateRootId = aggregateRootId,
                StartOffset = startOffset,
                EndOffset = endOffset
            }, token);
            if (records.IsEmpty())
            {
                return null;
            }

            var domainEvents = new List<IDomainEvent>();

            foreach (var record in records)
            {
                var domainEvent = DomainEventRecordPortAdapter.ToDomainEvent(record, _typeResolver, _binarySerializer);
                if (!domainEvent.IsValid())
                {
                    _logger.LogCritical($"Found illegal domain event from mysql state backend: {domainEvent.Print()}.");

                    continue;
                }

                domainEvents.Add(domainEvent);
            }

            return domainEvents;
        }

        public async Task<DomainEventMetrics> StatMetricsAsync(
            string aggregateRootId,
            int generation,
            CancellationToken token = default)
        {
            PreConditions.NotNullOrEmpty(aggregateRootId, nameof(aggregateRootId));
            PreConditions.Nonnegative(generation, nameof(generation));

            var tables = _options.DomainEventOptions.Tables;
            var sql = $"SELECT `DomainEventPayloadBytes` AS `UnCheckpointedBytes` FROM `{tables.DomainEventIndices}` WHERE `AggregateRootId`=@AggregateRootId AND AggregateRootGeneration>=@Generation";

            var records = await _db.ReadAsync<DomainEventMetrics>(sql, new
            {
                AggregateRootId = aggregateRootId,
                Generation = generation
            }, token);

            if (records.IsEmpty())
            {
                return DomainEventMetrics.Empty;
            }

            var aggregate = new DomainEventMetrics();
            foreach (var record in records)
            {
                aggregate.UnCheckpointedCount++;
                aggregate.UnCheckpointedBytes += record.UnCheckpointedBytes;
            }

            return aggregate;
        }
    }
}
