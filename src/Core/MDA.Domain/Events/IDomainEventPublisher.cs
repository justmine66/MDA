using MDA.Domain.Shared.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public interface IDomainEventPublisher
    {
        void Publish(IDomainEvent @event);

        void Publish<TAggregateRootId>(IDomainEvent<TAggregateRootId> @event);

        Task PublishAsync(IDomainEvent @event,CancellationToken token);

        Task PublishAsync<TAggregateRootId>(IDomainEvent<TAggregateRootId> @event, CancellationToken token);
    }
}
