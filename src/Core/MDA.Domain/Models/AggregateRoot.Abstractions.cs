using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using MDA.Domain.Notifications;

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
        /// 版本
        /// </summary>
        long Version { get; set; }
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
        /// 获取当前变更的领域事件列表
        /// </summary>
        IEnumerable<IDomainEvent> Events { get; }

        /// <summary>
        /// 重放变更的领域事件
        /// </summary>
        /// <param name="events">领域事件列表</param>
        void ReplayDomainEvents(IEnumerable<IDomainEvent> events);

        /// <summary>
        /// 应用领域事件。
        /// 1. 添加到领域事件列表；
        /// 2. 填充事件信息到领域模型。
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
        void OnDomainCommand(IDomainCommand command);

        /// <summary>
        /// 填充事件信息到领域模型。
        /// </summary>
        /// <param name="@event">领域事件</param>
        void OnDomainEvent(IDomainEvent @event);
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

    public abstract class EventSourcedAggregateRoot : IEventSourcedAggregateRoot
    {
        private readonly int _version;
        private readonly IList<IDomainEvent> _changes;

        protected EventSourcedAggregateRoot() { }

        protected EventSourcedAggregateRoot(string id, int version = 1)
        {
            Id = id;
            _changes = new List<IDomainEvent>();
            _version = version;
        }

        public IEnumerable<IDomainEvent> Events => _changes;
        public string Id { get; set; }
        public long Version { get; set; }

        public virtual void ApplyDomainEvent(IDomainEvent @event)
        {
            throw new NotImplementedException();
        }

        public virtual void OnDomainCommand(IDomainCommand command)
        {
            throw new NotImplementedException();
        }

        public virtual void OnDomainEvent(IDomainEvent @event)
        {
            throw new NotImplementedException();
        }

        public virtual void PublishDomainNotification(IDomainNotification notification)
        {
            throw new NotImplementedException();
        }

        public virtual void ReplayDomainEvents(IEnumerable<IDomainEvent> events)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class EventSourcedAggregateRoot<TId> : EventSourcedAggregateRoot, IEventSourcedAggregateRoot<TId>
    {
        private readonly int _version;
        private readonly IList<IDomainEvent> _changes;

        protected EventSourcedAggregateRoot(TId id, int version = 1) : base(id?.ToString(), version)
        {
            Id = id;
            _changes = new List<IDomainEvent>();
            _version = version;
        }

        public new TId Id { get; set; }

        protected virtual bool Equals(IEventSourcedAggregateRoot<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;

            return Id.Equals(other.Id) && Version == other.Version;
        }

        public virtual bool Equals(IAggregateRoot<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;

            return other is IEventSourcedAggregateRoot<TId> it && Equals(it);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return obj is IEntity<TId> other && Equals(other);
        }

        public override int GetHashCode()
            => HashHelper.ComputeHashCode(Id, Version);
    }

    public abstract class EventSourcedAggregateRootWithCompositeId<TId> :
        EventSourcedAggregateRoot<TId>,
        IAggregateRootWithCompositeId<TId>
    {
        protected EventSourcedAggregateRootWithCompositeId(TId id)
            : this(id, 1)
        { }

        protected EventSourcedAggregateRootWithCompositeId(TId id, int version)
            : base(id, version)
        { }

        public abstract IEnumerable<object> GetIdentityMembers();

        protected override bool Equals(IEventSourcedAggregateRoot<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;

            return other is IAggregateRootWithCompositeId<TId> it &&
                   GetIdentityMembers().SequenceEqual(it.GetIdentityMembers());
        }
    }
}
