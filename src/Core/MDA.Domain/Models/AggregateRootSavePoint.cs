using System;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 表示聚合根一次完整的快照保存点
    /// </summary>
    public class AggregateRootSavePoint<TAggregateRoot>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
        public AggregateRootSavePoint(TAggregateRoot aggregateRoot, int generation = 1)
        {
            AggregateRoot = aggregateRoot;
            LastModified = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Generation = generation;
        }

        /// <summary>
        /// 聚合根
        /// </summary>
        public TAggregateRoot AggregateRoot { get; set; }

        /// <summary>
        /// 最后更新时间戳
        /// </summary>
        public long LastModified { get; set; }

        /// <summary>
        /// 第几代
        /// </summary>
        public int Generation { get; set; }

        public AggregateRootSavePoint<TAggregateRoot> Refresh(TAggregateRoot aggregateRoot)
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
