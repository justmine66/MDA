using MDA.Domain.Models;

namespace MDA.Domain.Events
{
    public class DomainEventMetrics
    {
        public static DomainEventMetrics Empty = new DomainEventMetrics();

        /// <summary>
        /// 未设置检查点的容量，单位：字节。
        /// </summary>
        public long UnCheckpointedBytes { get; set; }

        /// <summary>
        /// 未设置检查点的数量。
        /// </summary>
        public long UnCheckpointedCount { get; set; }
    }

    public static class DomainEventMetricsExtensions
    {
        public static bool TriggerCheckpoint(this DomainEventMetrics metrics, CheckpointTriggerOptions options)
        {
            return metrics.UnCheckpointedBytes >= options.UnCheckpointedBytes ||
                   metrics.UnCheckpointedCount >= options.UnCheckpointedCount;
        }
    }
}
