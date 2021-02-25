using MDA.Domain.Shared.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    public interface IDomainNotificationPublisher
    {
        void Publish(IDomainNotification notification);

        void Publish<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification);

        Task PublishAsync(IDomainNotification notification, CancellationToken token = default);

        Task PublishAsync<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification, CancellationToken token = default);
    }
}
