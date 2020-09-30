using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Domain.Shared;
using MDA.MessageBus;
using MDA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.Domain.Models
{
    public abstract class EventSourcedAggregateRoot : IEventSourcedAggregateRoot, IEquatable<IEventSourcedAggregateRoot>
    {
        private Type ThisType => GetType();
        private IList<IDomainEvent> _mutatingDomainEvents;

        protected EventSourcedAggregateRoot() => _mutatingDomainEvents = new List<IDomainEvent>();

        protected EventSourcedAggregateRoot(string id, int version = 1)
        {
            Id = id;
            _mutatingDomainEvents = new List<IDomainEvent>();
            Version = version;
        }

        public IEnumerable<IDomainEvent> MutatingDomainEvents => _mutatingDomainEvents;
        public string Id { get; set; } = string.Empty;
        public int Version { get; set; } = 1;

        public virtual void ApplyDomainEvent(IDomainEvent @event)
        {
            // 1. 填充事件信息到模型
            if (@event == null) return;

            OnDomainEvent(@event);

            // 2. 添加到变更事件流
            if (_mutatingDomainEvents == null)
            {
                Version = 1;
                _mutatingDomainEvents = new List<IDomainEvent>();
            }

            _mutatingDomainEvents.Add(@event);
        }

        public virtual void OnDomainCommand(IDomainCommand command)
        {
            Version++;
            Id = command.AggregateRootId;

            ExecuteDomainMessage(DomainMessageType.Command, "HandleDomainCommand", command);
        }

        public virtual void OnDomainEvent(IDomainEvent @event)
        {
            @event.AggregateRootId = Id;
            @event.AggregateRootType = ThisType;
            @event.Version = Version;

            ExecuteDomainMessage(DomainMessageType.Event, "HandleDomainEvent", @event);
        }

        public virtual void PublishDomainNotification(IDomainNotification notification)
        {
            throw new NotImplementedException();
        }

        public virtual void ReplayDomainEvents(IEnumerable<IDomainEvent> events)
        {
            if (events == null) return;

            foreach (var @event in events)
            {
                OnDomainEvent(@event);
            }
        }

        private void ExecuteDomainMessage(DomainMessageType messageType, string methodName, IMessage message)
        {
            var methods = ThisType.GetRuntimeMethods();
            if (methods.IsEmpty())
            {
                return;
            }

            foreach (var method in methods)
            {
                if (method.Name != methodName) continue;

                var parameters = method.GetParameters();
                var parameterType = parameters[0].ParameterType;

                switch (messageType)
                {
                    case DomainMessageType.Command:
                        if (typeof(IDomainCommand).IsAssignableFrom(parameterType) &&
                            parameterType == message.GetType())
                        {
                            method.Invoke(this, new object[] { message });

                            return;
                        }
                        break;
                    case DomainMessageType.Event:
                        if (typeof(IDomainEvent).IsAssignableFrom(parameterType) &&
                            parameterType == message.GetType())
                        {
                            method.Invoke(this, new object[] { message });

                            return;
                        }
                        break;
                }
            }
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
            if (obj.GetType() != ThisType) return false;

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
        protected EventSourcedAggregateRoot(TId id, int version = 1)
            : base(id?.ToString(), version)
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
        protected EventSourcedAggregateRootWithCompositeId() { }
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
