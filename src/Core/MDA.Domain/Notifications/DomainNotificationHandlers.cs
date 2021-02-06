using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    public interface IDomainNotificationHandler<in TDomainNotification>
        where TDomainNotification : IDomainNotification
    {
        void OnDomainNotification(IDomainNotifyingContext context, TDomainNotification notification);
    }

    public interface IAsyncDomainNotificationHandler<in TDomainNotification>
        where TDomainNotification : IDomainNotification
    {
        Task OnDomainNotificationAsync(IDomainNotifyingContext context, TDomainNotification notification, CancellationToken token = default);
    }
}
