using System.Collections.Generic;

namespace MDA.EventSourcing
{
    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        private readonly List<IDomainEvent> _mutatingEvents;

        protected EventSourcedRootEntity()
        {
            _mutatingEvents = new List<IDomainEvent>();
        }

        /// <summary>
        /// Tapping into the event streams and populating their own models.
        /// </summary>
        /// <param name="eventStream">The event stream.</param>
        /// <param name="streamVersion">the stream version.</param>
        protected EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : this()
        {
            foreach (var e in eventStream) OnDomainEvent(e);
            UnMutatedVersion = streamVersion;
        }

        protected int MutatedVersion => UnMutatedVersion + 1;
        protected int UnMutatedVersion { get; }
        protected IEnumerable<IDomainEvent> GetMutatingEvents() => _mutatingEvents.ToArray();

        /// <summary>
        /// 填充事件信息到领域模型。
        /// </summary>
        /// <param name="e">领域事件</param>
        public void OnDomainEvent(dynamic e)
        {
            (this as dynamic).OnDomainEvent(e);
        }

        /// <summary>
        /// 应用领域事件。
        /// </summary>
        /// <remarks>1. 添加到领域事件列表；2. 填充事件信息到领域模型。</remarks>
        /// <param name="e">领域事件</param>
        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            OnDomainEvent(e);
        }
    }
}
