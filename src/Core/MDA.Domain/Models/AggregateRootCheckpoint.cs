using MDA.Infrastructure.Serialization;
using System;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 聚合根检查点
    /// 表示聚合根某一个时刻的一次完整快照
    /// </summary>
    public class AggregateRootCheckpoint<TPayload>
        where TPayload : ISerializationMetadataProvider
    {
        public AggregateRootCheckpoint(
            string aggregateRootId,
            Type aggregateRootType,
            int aggregateRootGeneration,
            long aggregateRootVersion,
            TPayload payload)
        {
            AggregateRootId = aggregateRootId;
            AggregateRootType = aggregateRootType;
            AggregateRootGeneration = aggregateRootGeneration;
            AggregateRootVersion = aggregateRootVersion;
            Payload = payload;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public string AggregateRootId { get; }

        /// <summary>
        /// 聚合根类型完全限定名
        /// </summary>
        public Type AggregateRootType { get; }

        /// <summary>
        /// 第几代聚合根,随着检查点(Checkpoint)快照而递增.
        /// </summary>
        public int AggregateRootGeneration { get; }

        /// <summary>
        /// 聚合根版本
        /// </summary>
        public long AggregateRootVersion { get; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 有效载荷
        /// </summary>
        public TPayload Payload { get; set; }

        public override string ToString()
            => $"AggregateRootId: {AggregateRootId}, AggregateRootType: {AggregateRootType.FullName}, AggregateRootGeneration: {AggregateRootGeneration}, AggregateRootVersion: {AggregateRootVersion}, Timestamp: {Timestamp}";
    }
}
