using System;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 表示聚合根一次完整的快照保存点
    /// </summary>
    public class AggregateRootSavePoint<TAggregateRoot>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
        public AggregateRootSavePoint(TAggregateRoot aggregateRoot, long domainEventOffset = 0)
        {
            AggregateRoot = aggregateRoot;
            DomainEventOffset = domainEventOffset;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 聚合根
        /// </summary>
        public TAggregateRoot AggregateRoot { get; set; }

        /// <summary>
        /// 领域事件位点
        /// </summary>
        public long DomainEventOffset { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
    }
}
