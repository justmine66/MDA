﻿using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Infrastructure.Serialization;
using System;
using System.Collections.Generic;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 聚合根
    /// </summary>
    public interface IAggregateRoot : ISerializationMetadataProvider
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 第几代
        /// 缘起：使聚合根像人一样分代，以实现从最近一代还原聚合根，比如：父母是最近的上一代，他们知道子女的名字，而上N代(祖父、曾祖父等)根本就不知道我们。
        /// 实现：随着检查点(Checkpoint)快照而递增。
        /// </summary>
        int Generation { get; set; }

        /// <summary>
        /// 修订版次，即修改聚合根状态，则递增版次。
        /// 缘起：在业务流程的最后一公里，希望乐观地控制状态变更的并发。
        /// 实现：领域事件记录了聚合根状态的业务变更序列，即只要用户发布领域事件，则递增版次，从而实现多版本并发控制<em>(Multi-Version Concurrency Control)</em>。
        /// </summary>
        long Version { get; set; }
    }

    /// <summary>
    /// 聚合根
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
    /// 聚合根
    /// </summary>
    public interface IEventSourcedAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// 获取当前变更领域事件列表
        /// </summary>
        IList<IDomainEvent> MutatingDomainEvents { get; }

        /// <summary>
        ///  获取当前变更领域通知列表
        /// </summary>
        IList<IDomainNotification> MutatingDomainNotifications { get; }

        /// <summary>
        /// 重放变更的领域事件
        /// </summary>
        /// <param name="events">领域事件列表</param>
        void ReplayDomainEvents(IEnumerable<IDomainEvent> events);

        /// <summary>
        /// 处理领域命令
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="command">领域命令</param>
        /// <returns>处理结果</returns>
        DomainCommandResult HandleDomainCommand(AggregateRootMessagingContext context, IDomainCommand command);

        /// <summary>
        /// 处理领域事件。
        /// </summary>
        /// <param name="@event">领域事件</param>
        void HandleDomainEvent(IDomainEvent @event);
    }

    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IEventSourcedAggregateRoot<TId> :
        IEventSourcedAggregateRoot,
        IAggregateRoot<TId>
    {
        /// <summary>
        /// 应用领域事件。
        /// 1. 填充事件信息到领域模型。
        /// 2. 添加到领域事件列表；
        /// </summary>
        /// <param name="@event">领域事件</param>
        void ApplyDomainEvent(IDomainEvent<TId> @event);
    }

    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IEventSourcedAggregateRootWithCompositeId<TId> :
        IEventSourcedAggregateRoot<TId>,
        IAggregateRootWithCompositeId<TId>
    { }
}
