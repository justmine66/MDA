using System;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 表示聚合根一次完整的快照检查点
    /// </summary>
    public class AggregateRootCheckpoint
    {
        public AggregateRootCheckpoint(IEventSourcedAggregateRoot aggregateRoot, int generation = 0)
        {
            AggregateRoot = aggregateRoot;
            LastModified = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Generation = generation;
        }

        /// <summary>
        /// 聚合根
        /// </summary>
        public IEventSourcedAggregateRoot AggregateRoot { get; set; }

        /// <summary>
        /// 最后更新时间戳
        /// </summary>
        public long LastModified { get; set; }

        /// <summary>
        /// 第几代
        /// </summary>
        public int Generation { get; set; }

        public AggregateRootCheckpoint Refresh(IEventSourcedAggregateRoot aggregateRoot)
        {
            AggregateRoot = aggregateRoot;
            LastModified = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Generation++;

            return this;
        }

        public override string ToString()
            => $"AggregateRootId: {AggregateRoot.Id},AggregateRootType: {AggregateRoot.GetType()}, Generation: {Generation}, LastModified: {LastModified}";
    }
}
