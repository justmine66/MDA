using MDA.Eventing;
using System.Collections.Generic;

namespace MDA.Domain.Model
{
    /// <summary>
    /// 表示一个基于事件溯源的聚合根实体。
    /// </summary>
    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        private readonly List<IDomainEvent> _mutatingEvents;

        private int _unmutatedVersion;
        protected int MutatedVersion
        {
            get { return _unmutatedVersion + 1; }
        }

        protected int UnmutatedVersion
        {
            get { return _unmutatedVersion; }
        }
    }
}
