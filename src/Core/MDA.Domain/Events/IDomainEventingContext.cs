using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Events;
using MDA.Domain.Shared.Exceptions;
using MDA.Domain.Shared.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    /// <summary>
    /// 领域事件处理上下文
    /// </summary>
    public interface IDomainEventingContext
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        void SetDomainEvent(IDomainEvent domainEvent);

        void PublishDomainCommand(IDomainCommand command);

        void PublishDomainCommand<TAggregateRootId>(IDomainCommand<TAggregateRootId> command);

        void PublishDomainNotification(IDomainNotification notification);

        void PublishDomainNotification<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification);

        Task PublishDomainNotificationAsync(IDomainNotification notification, CancellationToken token = default);

        Task PublishDomainNotificationAsync<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification, CancellationToken token = default);

        void PublishDomainExceptionMessage(IDomainExceptionMessage exception);

        Task PublishDomainExceptionMessageAsync(IDomainExceptionMessage exception, CancellationToken token = default);
    }
}
