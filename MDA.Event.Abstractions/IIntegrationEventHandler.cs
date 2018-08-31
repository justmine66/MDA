using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> 
        : IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent @event);
    }
}
