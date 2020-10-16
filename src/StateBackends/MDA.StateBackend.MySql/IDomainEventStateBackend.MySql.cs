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
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.MySql
{
    public class MySqlDomainEventStateBackend : IDomainEventStateBackend
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;
        private readonly IBinarySerializer _binarySerializer;
        private readonly ITypeResolver _typeResolver;
        private readonly MySqlStateBackendOptions _options;

        public MySqlDomainEventStateBackend(
            IRelationalDbStorageFactory factory,
            ILogger<MySqlDomainEventStateBackend> logger,
            ITypeResolver typeResolver,
            IOptions<MySqlStateBackendOptions> options,
            IBinarySerializer binarySerializer)
        {
            _logger = logger;
            _db = factory.CreateRelationalDbStorage();
            _typeResolver = typeResolver;
            _binarySerializer = binarySerializer;
            _options = options.Value;
        }

        public async Task AppendAsync(
            IDomainEvent @event,
            CancellationToken token = default)
        {
            if (!@event.IsValid())
            {
                _logger.LogError($"The domain event: [{@event.Print()}] cannot be stored mysql state backend.");

                return;
            }

            var domainEventRecord = DomainEventRecordPortAdapter.ToDomainEventRecord(@event, _binarySerializer);
            var parameters = DbParameterProvider.GetDbParameters(domainEventRecord);

            var tables = _options.DomainEventOptions.Tables;
            var insertDomainEventSql = $"INSERT INTO `{tables.DomainEventsIndices}`(`DomainCommandId`,`DomainCommandTypeFullName`,`DomainCommandVersion`,`AggregateRootId`,`AggregateRootTypeFullName`,`AggregateRootVersion`,`DomainEventId`,`DomainEventTypeFullName`,`DomainEventVersion`) VALUES(@DomainCommandId,@DomainCommandTypeFullName,@DomainCommandVersion,@AggregateRootId,@AggregateRootTypeFullName,@AggregateRootVersion,@DomainEventId,@DomainEventTypeFullName,@DomainEventVersion)";
            var insertDomainEventPayloadSql = $"INSERT INTO `{tables.DomainEventPayloads}`(`DomainEventId`,`Payload`) VALUES (@DomainEventId,@Payload)";

            try
            {
                await _db.ExecuteAsync($"{insertDomainEventSql};{insertDomainEventPayloadSql};", command => command.Parameters.AddRange(parameters), token);
            }
            catch (Exception ex)
            {
                if (TryCheckDuplicateEntryException(domainEventRecord.AggregateRootTypeFullName, ex)) return;

                _logger.LogError($"Append domain event has unknown exception: {ex}");
            }
        }

        public async Task AppendAsync(
            IEnumerable<IDomainEvent> events,
            CancellationToken token = default)
        {
            if (@events == null)
            {
                return;
            }

            foreach (var @event in events)
            {
                await AppendAsync(@event, token);
            }
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
            if (string.IsNullOrWhiteSpace(aggregateRootId) ||
                startOffset < 0)
            {
                return null;
            }

            var tables = _options.DomainEventOptions.Tables;

            var sql = $"SELECT d.`DomainCommandId`,d.`DomainCommandTypeFullName`,d.`DomainCommandVersion`,d.`AggregateRootId`,d.`AggregateRootTypeFullName`,d.`AggregateRootVersion`,d.`DomainEventId`,d.`DomainEventTypeFullName`,d.`DomainEventVersion`,d.`CreatedTimestamp`, p.`Payload` FROM `{tables.DomainEventsIndices}` d LEFT JOIN `{tables.DomainEventPayloads}` p ON d.`DomainEventId`=p.`DomainEventId` WHERE d.`AggregateRootId`=@AggregateRootId AND d.AggregateRootVersion>=@StartOffset AND d.AggregateRootVersion<@EndOffset";

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

        protected bool TryCheckDuplicateEntryException(string name, Exception ex)
        {
            if (ex is MySqlException inner && inner.HasDuplicateEntry())
            {
                // 事件已被处理
                _logger.LogWarning($"{name}: [已忽略]发现领域事件被重复处理：{inner.Message}");

                return true;
            }

            return false;
        }
    }
}
