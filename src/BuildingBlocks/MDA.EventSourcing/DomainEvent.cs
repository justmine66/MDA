using System;

namespace MDA.EventSourcing
{
    public abstract class DomainEvent : IDomainEvent
    {
        public string Principal { get; set; }
        public long EventVersion { get; set; }
        public DateTime OccurredOn { get; set; } = DateTime.Now;
    }
}
