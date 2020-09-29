using MDA.Shared.Utils;
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

        public async Task AppendAsync(
            IDomainEvent @event, 
            CancellationToken token = default)
        {
            if (@event != null)
            {
                _dict.AddOrUpdate(@event.AggregateRootId, new List<IDomainEvent>() { @event },
                    (key, oldValue) =>
                    {
                        oldValue.Add(@event);
                        return oldValue;
                    });
            }


            await Task.CompletedTask;
        }

        public async Task AppendAsync(
            IEnumerable<IDomainEvent> events, 
            CancellationToken token = default)
        {
            if (events.IsNotEmpty())
            {
                var eventsOfAggregate = events.GroupBy(it => it.AggregateRootId);

                foreach (var group in eventsOfAggregate)
                {
                    foreach (var @event in group)
                    {
                        _dict.AddOrUpdate(@event.AggregateRootId, new List<IDomainEvent>() { @event },
                            (key, oldValue) =>
                            {
                                oldValue.Add(@event);
                                return oldValue;
                            });
                    }
                }
            }

            await Task.CompletedTask;
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
                return events.SkipWhile(it => it.Version < startOffset);
            }

            return  await Task.FromResult(Enumerable.Empty<IDomainEvent>());
        }
    }
}
