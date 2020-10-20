using MDA.Domain.Events;
using MDA.Shared.Serialization;
using MDA.Shared.Types;
using MDA.Shared.Utils;
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
            var parameters = DbParameterProvider.GetDbParameters(domainEventRecord);

            var tables = _options.DomainEventOptions.Tables;
            var insertDomainEventSql = $"INSERT INTO `{tables.DomainEventsIndices}`(`DomainCommandId`,`DomainCommandType`,`DomainCommandVersion`,`AggregateRootId`,`AggregateRootType`,`AggregateRootVersion`,`AggregateRootGeneration`,`DomainEventId`,`DomainEventType`,`DomainEventVersion`,CreatedTimestamp) VALUES(@DomainCommandId,@DomainCommandType,@DomainCommandVersion,@AggregateRootId,@AggregateRootType,@AggregateRootVersion,@AggregateRootGeneration,@DomainEventId,@DomainEventType,@DomainEventVersion,@CreatedTimestamp)";
            var insertDomainEventPayloadSql = $"INSERT INTO `{tables.DomainEventPayloads}`(`DomainEventId`,`DomainEventVersion`,`Payload`) VALUES (@DomainEventId,@DomainEventVersion,@Payload)";

            try
            {
                var expectedRows = 2;
                var affectedRows = await _db.ExecuteAsync($"{insertDomainEventSql};{insertDomainEventPayloadSql};", command => command.Parameters.AddRange(parameters), token);
                if (affectedRows != expectedRows)
                    return DomainEventResult.StorageFailed(@event.Id,
                        $"The affected rows returned MySql state backend is incorrect, expected: {expectedRows}, actual: {affectedRows}.");

                await _eventPublisher.PublishAsync(@event, token);

                return DomainEventResult.StorageSucceed(@event.Id);
            }
            catch (Exception ex)
            {
                if (TryCheckDuplicateEntryException(domainEventRecord.AggregateRootType, ex, out var message))
                    return DomainEventResult.StorageSucceed(@event.Id, message);

                return DomainEventResult.StorageFailed(@event.Id, $"Append domain event has unknown exception: {ex}.");
            }
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
            int generation = 0,
            long startOffset = 0,
            CancellationToken token = default)
            => await GetEventStreamAsync(
                aggregateRootId,
                generation,
                startOffset,
                long.MaxValue,
                token);

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            int generation,
            long startOffset = 0,
            long endOffset = long.MaxValue,
            CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId) ||
                startOffset < 0)
            {
                return null;
            }

            var tables = _options.DomainEventOptions.Tables;

            var sql = $"SELECT d.`DomainCommandId`,d.`DomainCommandType`,d.`DomainCommandVersion`,d.`AggregateRootId`,d.`AggregateRootType`,d.`AggregateRootVersion`,d.`AggregateRootGeneration`,d.`DomainEventId`,d.`DomainEventType`,d.`DomainEventVersion`,d.`CreatedTimestamp`, p.`Payload` FROM `{tables.DomainEventsIndices}` d LEFT JOIN `{tables.DomainEventPayloads}` p ON d.`DomainEventId`=p.`DomainEventId` WHERE d.`AggregateRootId`=@AggregateRootId AND d.AggregateRootGeneration>=@Generation AND d.AggregateRootVersion>=@StartOffset AND d.AggregateRootVersion<@EndOffset";

            var records = await _db.ReadAsync<DomainEventRecord>(sql, new
            {
                AggregateRootId = aggregateRootId,
                Generation = generation,
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

        protected bool TryCheckDuplicateEntryException(string name, Exception ex, out string message)
        {
            if (ex is MySqlException inner && inner.HasDuplicateEntry())
            {
                // 事件已被处理
                message = $"{name}: [Ignored]find duplicated domain event from mysql state backend：{inner.Message}.";

                return true;
            }

            message = string.Empty;

            return false;
        }
    }
}
