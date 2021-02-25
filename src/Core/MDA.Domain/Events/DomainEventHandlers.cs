using MDA.Domain.Shared.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        void OnDomainEvent(IDomainEventingContext context, TDomainEvent @event);
    }

    public interface IAsyncDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task OnDomainEventAsync(IDomainEventingContext context, TDomainEvent @event, CancellationToken token = default);
    }
}
