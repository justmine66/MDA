namespace MDA.EventStore.SqlServer
{
    public class SqlServerDomainEventStoreOptions
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// 聚合根分片表数量。默认值：5。
        /// </summary>
        /// <remarks>
        /// 根据算法 hash(AggregateRootShardTableCount)，聚合根产生的领域事件数据将均匀地储存到分片表中。
        /// </remarks>
        public int AggregateRootShardTableCount { get; set; }

        /// <summary>
        /// 批量附加一个聚合根的所有事件(事件流)超时时间。单位秒。默认30秒。
        /// </summary>
        public int AppendEventStreamTimeoutInSeconds { get; set; }
    }
}
