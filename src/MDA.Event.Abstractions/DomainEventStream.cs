using System.Collections.Generic;
using System.Linq;

namespace MDA.Event.Abstractions
{
    public class DomainEventStream
    {
        public DomainEventStream(
            string aggregateRootId, 
            string aggregateRootTypeName, 
            IEnumerable<IDomainEvent> events)
        {
            AggregateRootId = aggregateRootId;
            AggregateRootTypeName = aggregateRootTypeName;
            Events = events;
        }

        public string AggregateRootId { get; private set; }
        public string AggregateRootTypeName { get; private set; }
        public IEnumerable<IDomainEvent> Events { get; private set; }

        public override string ToString()
        {
            return $"DomainEventStream [AggregateRootTypeName={AggregateRootTypeName},AggregateRootId={AggregateRootId},Events={string.Join("|", Events.Select(x => x.GetType().Name))}]";
        }
    }
}
