using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Event
{
    public interface IDomainEventBus
    {
        Task SendAsync<TDomainEvent>(TDomainEvent @event)
            where TDomainEvent : DomainEvent;

        Task SendAllAsync<TDomainEvent>(IEnumerable<TDomainEvent> events)
            where TDomainEvent : DomainEvent;
    }
}
