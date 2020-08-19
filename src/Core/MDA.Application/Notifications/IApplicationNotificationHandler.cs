using System.Threading.Tasks;

namespace MDA.Application.Notifications
{
    public interface IApplicationNotificationHandler<in TApplicationNotification>
        where TApplicationNotification : IApplicationNotification
    {
        void Handle(TApplicationNotification notification);
    }

    public interface IAsyncApplicationNotificationHandler<in TApplicationNotification>
        where TApplicationNotification : IApplicationNotification
    {
        Task HandleAsync(TApplicationNotification notification);
    }
}
