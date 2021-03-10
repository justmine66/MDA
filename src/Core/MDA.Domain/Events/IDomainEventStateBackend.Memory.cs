using MDA.Infrastructure.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public class MemoryDomainEventStateBackend : IDomainEventStateBackend
    {
        private readonly ConcurrentDictionary<string, List<IDomainEvent>> _dict;

        public MemoryDomainEventStateBackend()
        {
            _dict = new ConcurrentDictionary<string, List<IDomainEvent>>();
        }

        public async Task<DomainEventResult> AppendAsync(
            IDomainEvent @event,
            CancellationToken token = default)
        {
            if (@event == null)
                return await Task.FromResult(DomainEventResult.StorageSucceed(@event.Id));

            _dict.AddOrUpdate(@event.AggregateRootId, new List<IDomainEvent>() { @event },
                (key, oldValue) =>
                {
                    oldValue.Add(@event);
                    return oldValue;
                });

            return await Task.FromResult(DomainEventResult.StorageSucceed(@event.Id));
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
                _dict.AddOrUpdate(@event.AggregateRootId, new List<IDomainEvent>() { @event },
                    (key, oldValue) =>
                    {
                        oldValue.Add(@event);
                        return oldValue;
                    });

                results.Add(DomainEventResult.StorageSucceed(@event.Id));
            }

            return await Task.FromResult(results);
        }

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            long startOffset = 0,
            CancellationToken token = default)
            => await GetEventStreamAsync(aggregateRootId, startOffset, long.MaxValue, token);

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            long startOffset = 0,
            long endOffset = long.MaxValue,
            CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId))
            {
                return Enumerable.Empty<IDomainEvent>();
            }

            if (_dict.TryGetValue(aggregateRootId, out var events))
            {
                return events.SkipWhile(it => it.Version <= startOffset);
            }

            return await Task.FromResult(Enumerable.Empty<IDomainEvent>());
        }

        public async Task<DomainEventMetrics> StatMetricsAsync(string aggregateRootId, int generation, CancellationToken token = default)
        {
            return await Task.FromResult(new DomainEventMetrics());
        }
    }
}
