using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        void OnDomainEvent(IDomainEventHandlingContext context, TDomainEvent @event);
    }

    public interface IAsyncDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task OnDomainEventAsync(IDomainEventHandlingContext context, TDomainEvent @event, CancellationToken token = default);
    }
}
