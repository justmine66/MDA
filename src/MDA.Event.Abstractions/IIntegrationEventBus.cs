using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IIntegrationEventBus
    {
        Task SendAsync<TIIntegrationEvent>(TIIntegrationEvent @event)
            where TIIntegrationEvent : IIntegrationEvent;

        Task SendAllAsync<TIIntegrationEvent>(IEnumerable<TIIntegrationEvent> events)
            where TIIntegrationEvent : IIntegrationEvent;
    }
}
