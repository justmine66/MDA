using MDA.Domain.Events;
using MDA.Shared.Serialization;
using MDA.Shared.Types;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.MySql
{
    public class MySqlDomainEventStateBackend : IDomainEventStateBackend
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;
        private readonly IBinarySerializer _serializer;
        private readonly ITypeResolver _typeResolver;

        public MySqlDomainEventStateBackend(
            IRelationalDbStorageFactory factory,
            ILogger<MySqlDomainEventStateBackend> logger,
            IBinarySerializer serializer,
            ITypeResolver typeResolver)
        {
            _logger = logger;
            _db = factory.CreateRelationalDbStorage();
            _serializer = serializer;
            _typeResolver = typeResolver;
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

            var domainEventRecord = DomainEventRecordPortAdapter.ToDomainEventRecord(@event, _serializer);
            var parameters = DbParameterProvider.GetDbParameters(domainEventRecord, new Dictionary<string, string>()
            {
                ["Id"] = "DomainEventId",
                ["TypeFullName"] = "DomainEventTypeFullName",
                ["Version"] = "DomainEventVersion"
            });
            const string insertDomainEventSql = @"INSERT INTO `domain_events`(`DomainCommandId`,`DomainCommandTypeFullName`,`DomainCommandVersion`,`AggregateRootId`,`AggregateRootTypeFullName`,`AggregateRootVersion`,`DomainEventId`,`DomainEventTypeFullName`,`DomainEventVersion`) VALUES(@DomainCommandId,@DomainCommandTypeFullName,@DomainCommandVersion,@AggregateRootId,@AggregateRootTypeFullName,@AggregateRootVersion,@DomainEventId,@DomainEventTypeFullName,@DomainEventVersion)";
            const string insertDomainEventPayloadSql = @"INSERT INTO `domain_event_payloads`(`DomainEventId`,`Payload`) VALUES (@DomainEventId,@Payload)";

            await _db.ExecuteAsync($"{insertDomainEventSql};{insertDomainEventPayloadSql};", command => command.Parameters.AddRange(parameters), token);
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

            const string sql = "SELECT d.`DomainCommandId`,d.`DomainCommandTypeFullName`,d.`DomainCommandVersion`,d.`AggregateRootId`,d.`AggregateRootTypeFullName`,d.`AggregateRootVersion`,d.`DomainEventId`,d.`DomainEventTypeFullName`,d.`DomainEventVersion`, p.`Payload` FROM `domain_events` d LEFT JOIN `domain_event_payloads` p ON d.`DomainEventId`=p.`DomainEventId` WHERE d.AggregateRootVersion>@StartOffset AND d.AggregateRootVersion<@EndOffset";

            var records = await _db.ReadAsync<DomainEventRecord>(sql, token);
            if (records == null)
            {
                return null;
            }

            var domainEvents = new List<IDomainEvent>();

            foreach (var record in records)
            {
                var domainEvent = DomainEventRecordPortAdapter.ToDomainEvent(record, _typeResolver, _serializer);
                if (!domainEvent.IsValid())
                {
                    _logger.LogCritical($"Found illegal domain event from mysql state backend: {domainEvent.Print()}.");

                    continue;
                }

                domainEvents.Add(domainEvent);
            }

            return domainEvents;
        }
    }
}
