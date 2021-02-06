using MDA.Domain.Commands;
using MDA.Domain.Exceptions;
using MDA.Domain.Notifications;
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
