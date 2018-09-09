using MDA.Common.Domain.Model;
using System.Collections.Generic;

namespace MDA.Event.Abstractions
{
    /// <summary>
    /// 表示一个基于事件溯源的聚合根实体。
    /// </summary>
    /// <remarks>
    /// 1. 加载当前状态
    /// 当需要事件溯源实体执行变更状态的命令时，首先需要从事件储存中加载这个实体之前所有的事件，并应用于实体上，创建实体的当前状态。
    /// 2. 写入一致性
    /// 在创建好实体的状态后，执行请求命令所要求的业务逻辑。如果业务逻辑执行失败，就向客户端返回错误；如果执行成功，就发出新的事件。所以，我们必须保证将所有的事件保存到事件存储中，同时保证在此期间没有其他与这个实体相关的事件被写入，否则可能破坏领域对象的一致性。
    /// 2.1. 乐观并发控制
    /// 为实体添加字段<see cref="_unmutatedVersion"/>，只有当版本未变异时，才保存这个事件，即保证只保存版本相同的事件。
    /// 2.2. 单线程写入
    /// 确保串行化写入，即确保所有关于实体的写入都是发生在单线程上。
    /// </remarks>
    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        /// <summary>
        /// 初始化一个 <see cref="EventSourcedRootEntity"/> 实例。
        /// </summary>
        public EventSourcedRootEntity()
        {
            _mutatingEvents = new List<IDomainEvent>();
        }
        /// <summary>
        /// 初始化一个 <see cref="EventSourcedRootEntity"/> 实例。
        /// </summary>
        /// <param name="eventStream">事件流</param>
        /// <param name="streamVersion">流版本</param>
        public EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion)
           : this()
        {
            foreach (var evt in eventStream)
            {
                (this as dynamic).Handle(evt);
            }

            _unmutatedVersion = streamVersion;
        }

        /// <summary>
        /// 领域事件列表
        /// </summary>
        private readonly IList<IDomainEvent> _mutatingEvents;

        /// <summary>
        /// 已变异的版本号
        /// </summary>
        private int _unmutatedVersion;
        protected int MutatedVersion
        {
            get { return _unmutatedVersion + 1; }
        }

        /// <summary>
        /// 为变异的版本号
        /// </summary>
        protected int UnmutatedVersion
        {
            get { return _unmutatedVersion; }
        }

        /// <summary>
        /// 获取领域事件列表。
        /// </summary>
        /// <returns></returns>
        public IList<IDomainEvent> GetMutatingEvents()
        {
            return _mutatingEvents;
        }

        /// <summary>
        /// 应用领域事件
        /// </summary>
        /// <param name="e">领域事件</param>
        protected void Apply(dynamic domainEvent)
        {
            _mutatingEvents.Add(domainEvent);
            (this as dynamic).Handle(domainEvent);
        }
    }
}
