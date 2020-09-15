using MDA.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Notifications
{
    public interface IApplicationNotificationHandler<in TApplicationNotification> : IMessageHandler<TApplicationNotification>
        where TApplicationNotification : IApplicationNotification
    {
        new void Handle(TApplicationNotification notification);
    }

    public interface IAsyncApplicationNotificationHandler<in TApplicationNotification> : IAsyncMessageHandler<TApplicationNotification>
        where TApplicationNotification : IApplicationNotification
    {
        new Task HandleAsync(TApplicationNotification notification, CancellationToken token = default);
    }
}