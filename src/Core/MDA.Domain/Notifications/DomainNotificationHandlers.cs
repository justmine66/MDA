using MDA.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    public interface IDomainNotificationHandler<in TDomainNotification> : IMessageHandler<TDomainNotification>
        where TDomainNotification : IDomainNotification
    {
        new void Handle(TDomainNotification notification);
    }

    public interface IAsyncDomainNotificationHandler<in TDomainNotification> : IAsyncMessageHandler<TDomainNotification>
        where TDomainNotification : IDomainNotification
    {
        new Task HandleAsync(TDomainNotification notification, CancellationToken token = default);
    }
}
