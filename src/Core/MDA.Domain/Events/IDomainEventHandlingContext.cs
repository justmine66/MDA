using MDA.Domain.Commands;
using MDA.Domain.Exceptions;
using MDA.Domain.Notifications;
using System;

namespace MDA.Domain.Events
{
    /// <summary>
    /// 领域事件处理上下文
    /// </summary>
    public interface IDomainEventHandlingContext
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
        /// 领域通知发布器
        /// </summary>
        IDomainNotificationPublisher DomainNotificationPublisher { get; }

        /// <summary>
        /// 领域异常消息发布器
        /// </summary>
        IDomainExceptionMessagePublisher DomainExceptionMessagePublisher { get; }
    }
}
