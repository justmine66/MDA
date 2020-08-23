using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    public interface IDomainNotificationHandler<in TDomainNotification>
        where TDomainNotification : IDomainNotification
    {
        void Handle(TDomainNotification notification);
    }

    public interface IAsyncDomainNotificationHandler<in TDomainNotification>
        where TDomainNotification : IDomainNotification
    {
        Task HandleAsync(TDomainNotification notification, CancellationToken token = default);
    }
}
