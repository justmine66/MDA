using System;

namespace MDA.EventSourcing
{
    public abstract class DomainEvent : IDomainEvent
    {
        public BusinessPrincipal Principal { get; set; }
        public long EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public DateTime HandledOn { get; set; }
    }
}
