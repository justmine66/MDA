using System;

namespace MDA.StateBackend.MySql
{
    public class DomainEventRecord
    {
        /// <summary>
        /// 领域事件标识
        /// </summary>
        public string DomainEventId { get; set; }

        /// <summary>
        /// 领域事件版本
        /// </summary>
        public long DomainEventVersion { get; set; }

        /// <summary>
        /// 领域事件类型完全限定名
        /// </summary>
        public string DomainEventType { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        public string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名
        /// </summary>
        public string DomainCommandType { get; set; }

        /// <summary>
        /// 领域命令版本
        /// </summary>
        public long DomainCommandVersion { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名
        /// </summary>
        public string AggregateRootType { get; set; }

        /// <summary>
        /// 第几代聚合根,随着检查点(Checkpoint)快照而递增.
        /// </summary>
        public int AggregateRootGeneration { get; set; }

        /// <summary>
        /// 聚合根版本
        /// </summary>
        public long AggregateRootVersion { get; set; }

        /// <summary>
        /// 创建时间，时间戳，单位：毫秒。
        /// </summary>
        public long CreatedTimestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        /// <summary>
        /// 有效载荷
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// 领域事件载荷容量，单位：字节。
        /// </summary>
        public int DomainEventPayloadBytes => Payload.Length;
    }
}
