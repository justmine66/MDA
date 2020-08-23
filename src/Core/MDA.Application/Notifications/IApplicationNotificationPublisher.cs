using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Notifications
{
    public interface IApplicationNotificationPublisher
    {
        void Publish(IApplicationNotification notification);

        void Publish<TId>(IApplicationNotification<TId> notification);
    }

    public interface IAsyncApplicationNotificationPublisher : IApplicationNotificationPublisher
    {
        Task PublishAsync(IApplicationNotification notification);

        Task PublishAsync<TId>(IApplicationNotification<TId> notification, CancellationToken token = default);
    }
}
