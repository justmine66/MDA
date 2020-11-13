using MDA.Application.Notifications;
using MDA.Domain.Commands;
using System;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令上下文
    /// </summary>
    public interface IApplicationCommandContext
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 领域命令发布器
        /// </summary>
        IDomainCommandPublisher DomainCommandPublisher { get; }

        /// <summary>
        /// 应用层通知发布器
        /// </summary>
        IApplicationNotificationPublisher ApplicationNotificationPublisher { get; }
    }
}
