using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Infrastructure.Utils;
using MDA.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MDA.Domain.Models
{
    public abstract class EventSourcedAggregateRoot :
        IEventSourcedAggregateRoot,
        IEquatable<IEventSourcedAggregateRoot>
    {
        protected Type AggregateRootType => GetType();

        protected EventSourcedAggregateRoot()
        {
            MutatingDomainEvents = new List<IDomainEvent>();
            MutatingDomainNotifications = new List<IDomainNotification>();
        }

        protected EventSourcedAggregateRoot(string id, int version = 0)
        {
            Id = id;
            Version = version;
            MutatingDomainEvents = new List<IDomainEvent>();
            MutatingDomainNotifications = new List<IDomainNotification>();
        }

        public IList<IDomainEvent> MutatingDomainEvents { get; protected set; }
        public IList<IDomainNotification> MutatingDomainNotifications { get; protected set; }
        public string Id { get; set; } = string.Empty;
        public int Generation { get; set; }
        public long Version { get; set; }

        public virtual void ApplyDomainEvent(IDomainEvent @event)
        {
            // 1. 填充领域事件信息到聚合根
            if (@event == null) return;

            @event.AggregateRootId = Id;
            FillAggregateInfo(@event);
            HandleDomainEvent(@event);

            // 2. 添加到当前变更领域事件列表
            if (MutatingDomainEvents == null)
            {
                MutatingDomainEvents = new List<IDomainEvent>();
            }

            MutatingDomainEvents.Add(@event);
        }

        public virtual DomainCommandResult HandleDomainCommand(IDomainCommand command)
        {
            var aggregateRootId = command.AggregateRootId;
            var domainCommandAggregateType = command.AggregateRootType;

            if (AggregateRootType != domainCommandAggregateType)
            {
                return DomainCommandResult.Failed(command.Id, $"Incorrect the aggregate root type, expected: {AggregateRootType.FullName}, actual: {domainCommandAggregateType.FullName}");
            }

            if (!string.IsNullOrWhiteSpace(Id) && Id != aggregateRootId)
            {
                return DomainCommandResult.Failed(command.Id, $"Incorrect the aggregate root id, expected: {Id}, actual: {aggregateRootId}");
            }

            ExecuteDomainMessage(DomainMessageTypes.Command, command);

            return DomainCommandResult.Succeed(command.Id);
        }

        public virtual void HandleDomainEvent(IDomainEvent @event)
            => ExecuteDomainMessage(DomainMessageTypes.Event, @event);

        public virtual void PublishDomainNotification(IDomainNotification notification)
        {
            // 1. 填充领域通知信息
            notification.AggregateRootId = Id;
            notification.AggregateRootType = AggregateRootType;

            // 2. 添加到当前变更领域通知列表
            if (MutatingDomainNotifications == null)
            {
                MutatingDomainNotifications = new List<IDomainNotification>();
            }

            MutatingDomainNotifications.Add(notification);
        }

        public virtual void ReplayDomainEvents(IEnumerable<IDomainEvent> events)
        {
            if (events.IsEmpty()) return;

            foreach (var @event in events)
            {
                var eventAggregateType = @event.AggregateRootType;
                if (AggregateRootType != eventAggregateType)
                {
                    throw new NotSupportedException($"Incorrect the aggregate root type, expected: {AggregateRootType.FullName}, actual: {eventAggregateType.FullName}");
                }

                Id = @event.AggregateRootId;
                Version = @event.AggregateRootVersion;
                Generation = @event.AggregateRootGeneration;

                HandleDomainEvent(@event);
            }
        }

        private void ExecuteDomainMessage(DomainMessageTypes messageType, IMessage message)
        {
            var messageParameterType = message.GetType();
            var messageParameterTypeFullName = message.GetType();
            var aggregateTypeFullName = AggregateRootType.FullName;

            string methodName;
            switch (messageType)
            {
                case DomainMessageTypes.Command:
                    methodName = "OnDomainCommand";

                    var domainCommandType = typeof(IDomainCommand);

                    if (!domainCommandType.IsAssignableFrom(messageParameterType))
                    {
                        throw new MethodNotFoundException($"The method: {methodName}({messageParameterTypeFullName}) in {aggregateTypeFullName} is wrong, reason: {messageParameterTypeFullName} cannot assign to interface: {domainCommandType.FullName}.");
                    }

                    break;
                case DomainMessageTypes.Event:
                    methodName = "OnDomainEvent";

                    var domainEventType = typeof(IDomainEvent);

                    if (!domainEventType.IsAssignableFrom(messageParameterType))
                    {
                        throw new MethodNotFoundException($"The method: {methodName}({messageParameterTypeFullName}) in {aggregateTypeFullName} is wrong, reason: {messageParameterTypeFullName} cannot assign to interface: {domainEventType.FullName}.");
                    }

                    break;
                case DomainMessageTypes.Notification:
                    throw new NotSupportedException($"{messageType}");
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            var method = AggregateRootType
                .GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public,
                    null,
                    new[] { messageParameterType },
                    null);
            if (method == null)
            {
                throw new MethodNotFoundException($"No {methodName}({messageParameterTypeFullName}) found in {aggregateTypeFullName}.");
            }

            var instance = Expression.Constant(this);
            var parameter = Expression.Parameter(messageParameterType, methodName);
            var call = Expression.Call(instance, method, parameter);
            var lambda = Expression.Lambda(call, parameter);
            var methodDelegate = lambda.Compile();

            methodDelegate.DynamicInvoke(message);
        }

        protected void FillAggregateInfo(IDomainEvent @event)
        {
            @event.AggregateRootType = AggregateRootType;
            @event.AggregateRootGeneration = Generation;
            @event.AggregateRootVersion = ++Version;
            @event.Version++;
        }

        public bool Equals(IEventSourcedAggregateRoot other)
        {
            if (ReferenceEquals(null, other)) return false;

            return Id.Equals(other.Id) && Version == other.Version;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != AggregateRootType) return false;

            return obj is IEventSourcedAggregateRoot other && Equals(other);
        }

        public override int GetHashCode()
            => HashHelper.ComputeHashCode(Id, Version);

        public string[] IgnoreKeys => new[] { nameof(MutatingDomainEvents), nameof(MutatingDomainNotifications), nameof(IgnoreKeys) };
    }

    public abstract class EventSourcedAggregateRoot<TId> :
        EventSourcedAggregateRoot,
        IEventSourcedAggregateRoot<TId>
    {
        protected EventSourcedAggregateRoot()
            => base.Id = Id?.ToString();
        protected EventSourcedAggregateRoot(TId id, int version = 0)
            : base(id?.ToString(), version)
        {
            Id = id;
        }

        private TId _id;
        public new TId Id
        {
            get => _id;
            set
            {
                _id = value;

                base.Id = _id.ToString();
            }
        }

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

        public void ApplyDomainEvent(IDomainEvent<TId> @event)
        {
            // 1. 填充事件信息到聚合根
            if (@event == null) return;

            @event.AggregateRootId = Id;
            FillAggregateInfo(@event);
            HandleDomainEvent(@event);

            // 2. 添加到当前变更事件列表
            if (MutatingDomainEvents == null)
            {
                MutatingDomainEvents = new List<IDomainEvent>();
            }

            MutatingDomainEvents.Add(@event);
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
        protected EventSourcedAggregateRootWithCompositeId() { }
        protected EventSourcedAggregateRootWithCompositeId(TId id, int version = 0)
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
