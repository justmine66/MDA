using System;

namespace MDA.Core.EventSourcing
{
    public abstract class DomainEvent : IDomainEvent
    {
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
