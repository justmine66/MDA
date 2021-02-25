using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Exceptions;
using MDA.Domain.Shared.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Notifications
{
    /// <summary>
    /// 领域通知处理上下文
    /// </summary>
    public interface IDomainNotifyingContext
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        void SetDomainNotification(IDomainNotification notification);

        void PublishDomainCommand(IDomainCommand command);

        void PublishDomainCommand<TAggregateRootId>(IDomainCommand<TAggregateRootId> command);

        void PublishDomainExceptionMessage(IDomainExceptionMessage exception);

        Task PublishDomainExceptionMessageAsync(IDomainExceptionMessage exception, CancellationToken token = default);
    }
}
