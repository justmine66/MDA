using System;

namespace MDA.EventSourcing
{
    /// <summary>
    /// Captures the memory of something interesting which affects the domain.
    /// </summary>
    public interface IDomainEvent
    {
        BusinessPrincipal Principal { get; set; }
        long EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
        DateTime HandledOn { get; set; }
    }

    public class BusinessPrincipal
    {
        public string Id { get; set; }
        public string TypeName { get; set; }
    }
}
