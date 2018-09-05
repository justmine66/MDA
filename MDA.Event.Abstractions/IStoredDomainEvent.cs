using System;

namespace MDA.Event.Abstractions
{
    public class StoredDomainEvent
    {
        public string EventId { get; set; }
        public int EventVersion { get; set; }
        public string CommandId { get; set; }
        public string AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
        public DateTime? OccurredOn { get; set; }
    }
}
