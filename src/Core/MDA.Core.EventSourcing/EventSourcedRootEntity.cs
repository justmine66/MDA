using System.Collections.Generic;

namespace MDA.Core.EventSourcing
{
    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        private readonly List<IDomainEvent> _mutatingEvents;

        protected EventSourcedRootEntity()
        {
            _mutatingEvents = new List<IDomainEvent>();
        }

        protected EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : this()
        {
            foreach (var e in eventStream) OnDomainEvent(e);
            UnMutatedVersion = streamVersion;
        }

        protected int MutatedVersion => UnMutatedVersion + 1;
        protected int UnMutatedVersion { get; }
        protected IEnumerable<IDomainEvent> GetMutatingEvents() => _mutatingEvents.ToArray();

        protected void OnDomainEvent(IDomainEvent e)
        {
            (this as dynamic).Apply(e);
        }

        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            OnDomainEvent(e);
        }
    }
}
