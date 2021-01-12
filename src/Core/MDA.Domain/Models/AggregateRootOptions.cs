namespace MDA.Domain.Models
{
    /// <summary>
    /// 聚合根配置项
    /// </summary>
    public class AggregateRootOptions
    {
        /// <summary>
        /// 检查点触发配置项
        /// </summary>
        public CheckpointTriggerOptions CheckpointTriggerOptions { get; set; }

        /// <summary>
        /// 聚合根缓存配置项
        /// </summary>
        public AggregateRootCacheOptions CacheOptions { get; set; }

        /// <summary>
        /// 聚合根状态后端配置项
        /// </summary>
        public AggregateRootStateBackendOptions StateBackendOptions { get; set; }
    }

    /// <summary>
    /// 检查点触发配置项
    /// 注意：满足任一条件，都会触发检查点。
    /// </summary>
    public class CheckpointTriggerOptions
    {
        /// <summary>
        /// 步长，检查的间隔，单位：秒，默认：1小时。
        /// </summary>
        public int StepInSeconds { get; set; } = 3600;

        /// <summary>
        /// 未设置检查点的容量，单位：字节，默认：1G。
        /// </summary>
        public long UnCheckpointedBytes { get; set; } = 1024 * 1024 * 1024;

        /// <summary>
        /// 未设置检查点的数量，默认：1000条。
        /// </summary>
        public long UnCheckpointedCount { get; set; } = 10000;
    }

    /// <summary>
    /// 聚合根状态后端配置项
    /// </summary>
    public class AggregateRootStateBackendOptions
    {
        /// <summary>
        /// 每批提交变更中领域事件流的大小，默认：1000。
        /// 当聚合根产生的领域事件超过达到此阈值时，才持久化存储到状态后端。
        /// 当提交批尺寸为1时，表示每次应用完领域事件就马上持久化，此时延迟最低。
        /// </summary>
        public int SubmitBatchSize { get; set; } = 1000;

        /// <summary>
        /// 提交变更中领域事件流的延迟，单位：毫秒，默认：1000。
        /// 为了避免低流量下，变更中的领域事件无法达到提交批尺寸，故当产生时长超过该延迟时，便持久化存储到状态后端。
        /// 当提交延迟为负数时，表示完全按批尺寸提交，此时吞吐最高。
        /// </summary>
        public int SubmitDurationInMilliseconds { get; set; } = 1000;
    }

    /// <summary>
    /// 聚合根缓存配置项
    /// </summary>
    public class AggregateRootCacheOptions
    {
        /// <summary>
        /// 缓存容量上限，默认：物理内存的80%。
        /// </summary>
        public int MaxSize { get; set; } = int.MaxValue;

        /// <summary>
        /// 缓存条目活跃存活时长(Time To Live)，单位：秒，默认：1分钟，即一分钟内没有被访问，将从缓存中清理掉。
        /// </summary>
        public int TTL { get; set; } = 60;

        /// <summary>
        /// 缓存条目最大存活时长，单位：秒，默认：永久。
        /// </summary>
        public int MaxAge { get; set; } = int.MaxValue;
    }
}
