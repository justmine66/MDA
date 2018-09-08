using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IDomainEventBus
    {
        Task SendAsync<TIDomainEvent>(TIDomainEvent @event)
            where TIDomainEvent : IDomainEvent;

        Task SendAllAsync<TIDomainEvent>(IEnumerable<TIDomainEvent> events)
            where TIDomainEvent : IDomainEvent;
    }
}
