using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.MessageBus;
using MDA.Shared.Utils;
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
        private Type AggregateRootType => GetType();
        private IList<IDomainEvent> _mutatingDomainEvents;

        protected EventSourcedAggregateRoot() => _mutatingDomainEvents = new List<IDomainEvent>();

        protected EventSourcedAggregateRoot(string id, int version = 0)
        {
            Id = id;
            _mutatingDomainEvents = new List<IDomainEvent>();
            Version = version;
        }

        public IEnumerable<IDomainEvent> MutatingDomainEvents => _mutatingDomainEvents;
        public string Id { get; set; } = string.Empty;
        public int Generation { get; set; }
        public long Version { get; set; }

        public virtual void ApplyDomainEvent(IDomainEvent @event)
        {
            // 1. 填充事件信息到模型
            if (@event == null) return;

            @event.AggregateRootId = Id;
            @event.AggregateRootType = AggregateRootType;
            @event.AggregateRootGeneration = Generation;
            @event.AggregateRootVersion = ++Version;
            @event.Version++;

            HandleDomainEvent(@event);

            // 2. 添加到变更事件流
            if (_mutatingDomainEvents == null)
            {
                Version = 0;
                _mutatingDomainEvents = new List<IDomainEvent>();
            }

            _mutatingDomainEvents.Add(@event);
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

            ExecuteDomainMessage(DomainMessageType.Command, command);

            return DomainCommandResult.Succeed(command.Id);
        }

        public virtual void HandleDomainEvent(IDomainEvent @event)
        {
            ExecuteDomainMessage(DomainMessageType.Event, @event);
        }

        public virtual void PublishDomainNotification(IDomainNotification notification)
        {
            throw new NotImplementedException();
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

        private void ExecuteDomainMessage(DomainMessageType messageType, IMessage message)
        {
            var messageParameterType = message.GetType();
            var messageParameterTypeFullName = message.GetType();
            var aggregateTypeFullName = AggregateRootType.FullName;

            string methodName;
            switch (messageType)
            {
                case DomainMessageType.Command:
                    methodName = "OnDomainCommand";

                    var domainCommandType = typeof(IDomainCommand);

                    if (!domainCommandType.IsAssignableFrom(messageParameterType))
                    {
                        throw new MethodNotFoundException($"The method: {methodName}({messageParameterTypeFullName}) in {aggregateTypeFullName} is wrong, reason: {messageParameterTypeFullName} cannot assign to interface: {domainCommandType.FullName}.");
                    }

                    break;
                case DomainMessageType.Event:
                    methodName = "OnDomainEvent";

                    var domainEventType = typeof(IDomainEvent);

                    if (!domainEventType.IsAssignableFrom(messageParameterType))
                    {
                        throw new MethodNotFoundException($"The method: {methodName}({messageParameterTypeFullName}) in {aggregateTypeFullName} is wrong, reason: {messageParameterTypeFullName} cannot assign to interface: {domainEventType.FullName}.");
                    }

                    break;
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
    }

    public abstract class EventSourcedAggregateRoot<TId> : EventSourcedAggregateRoot, IEventSourcedAggregateRoot<TId>
    {
        private readonly int _version;
        private readonly IList<IDomainEvent> _changes;

        protected EventSourcedAggregateRoot()
            => base.Id = Id?.ToString();
        protected EventSourcedAggregateRoot(TId id, int version = 0)
            : base(id?.ToString(), version)
        {
            Id = id;
            _changes = new List<IDomainEvent>();
            _version = version;
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
