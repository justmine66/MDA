using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Notifications
{
    public interface IApplicationNotificationPublisher
    {
        void Publish<TApplicationNotification>(TApplicationNotification notification)
            where TApplicationNotification : IApplicationNotification;
    }

    public interface IAsyncApplicationNotificationPublisher 
    {
        Task PublishAsync<TApplicationNotification>(TApplicationNotification notification, CancellationToken token = default)
            where TApplicationNotification : IApplicationNotification;
    }
}
