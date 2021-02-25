using MDA.Application.Notifications;
using MDA.Domain.Shared.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令上下文
    /// </summary>
    public interface IApplicationCommandingContext
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        void SetApplicationCommand(IApplicationCommand command);

        void PublishDomainCommand(IDomainCommand command);

        void PublishDomainCommand<TAggregateRootId>(IDomainCommand<TAggregateRootId> command);

        void PublishApplicationNotification<TApplicationNotification>(TApplicationNotification notification)
            where TApplicationNotification : IApplicationNotification;

        Task PublishApplicationNotificationAsync<TApplicationNotification>(TApplicationNotification notification, CancellationToken token = default)
            where TApplicationNotification : IApplicationNotification;
    }
}
