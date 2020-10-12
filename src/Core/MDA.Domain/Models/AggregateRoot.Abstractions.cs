using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Notifications;
using System;
using System.Collections.Generic;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 聚合根，封装业务对象的不变性。
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 版本号
        /// 随着聚合根变更不断递增，以实现多版本并发控制(Mutil-Version Concurrency Control)机制。
        /// 即每次收到入站领域命令，版本号便加1。
        /// </summary>
        int Version { get; set; }
    }

    /// <summary>
    /// 聚合根，封装业务对象的不变性。
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IAggregateRoot<TId> : IAggregateRoot, IEquatable<IAggregateRoot<TId>>
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }

    public interface IAggregateRootWithCompositeId<TId> : IAggregateRoot<TId>
    {
        /// <summary>
        /// Gets all members of the identity of the aggregate root.
        /// </summary>
        IEnumerable<object> GetIdentityMembers();
    }

    /// <summary>
    /// 聚合根，封装业务对象的不变性。
    /// </summary>
    public interface IEventSourcedAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// 获取变更产生的领域事件列表
        /// </summary>
        IEnumerable<IDomainEvent> MutatingDomainEvents { get; }

        /// <summary>
        /// 重放变更的领域事件
        /// </summary>
        /// <param name="events">领域事件列表</param>
        void ReplayDomainEvents(IEnumerable<IDomainEvent> events);

        /// <summary>
        /// 应用领域事件。
        /// 1. 填充事件信息到领域模型。
        /// 2. 添加到领域事件列表；
        /// </summary>
        /// <param name="@event">领域事件</param>
        void ApplyDomainEvent(IDomainEvent @event);

        /// <summary>
        /// 发布领域通知。
        /// 应用场景：允许领域命令以非异常的方式发送预检结果。
        /// </summary>
        /// <param name="notification">领域通知</param>
        void PublishDomainNotification(IDomainNotification notification);

        /// <summary>
        /// 执行领域命令
        /// </summary>
        /// <param name="command">领域命令</param>
        void HandleDomainCommand(IDomainCommand command);

        /// <summary>
        /// 填充事件信息到领域模型。
        /// </summary>
        /// <param name="@event">领域事件</param>
        void HandleDomainEvent(IDomainEvent @event);
    }

    /// <summary>
    /// 聚合根，封装业务对象的不变性。
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IEventSourcedAggregateRoot<TId> : IEventSourcedAggregateRoot, IAggregateRoot<TId> { }

    /// <summary>
    /// 聚合根，封装业务对象的不变性。
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IEventSourcedAggregateRootWithCompositeId<TId> :
        IEventSourcedAggregateRoot<TId>,
        IAggregateRootWithCompositeId<TId>
    { }
}
