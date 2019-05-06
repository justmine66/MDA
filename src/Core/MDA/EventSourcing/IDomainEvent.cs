using System;

namespace MDA.EventSourcing
{
    public interface IDomainEvent
    {
        BusinessPrincipal Principal { get; set; }
        long EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
        DateTime ProcessingTime { get; set; }
    }

    public class BusinessPrincipal
    {
        public string Id { get; set; }
        public string TypeName { get; set; }
    }
}
