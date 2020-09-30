using System;

namespace MDA.Domain.Models
{
    public class AggregateRootOptions
    {
        public AggregateRootCacheOptions CacheOptions { get; set; }
        
        public AggregateRootStateBackendOptions BackendOptions { get; set; }
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
        /// 缓存容量上限，默认：int.MaxValue。
        /// </summary>
        public int MaxSize { get; set; } = int.MaxValue;

        /// <summary>
        /// 缓存条目最大存活时长(Time To Live)，单位：秒，默认：1分钟，即一分钟内没有被访问，将从缓存中清理掉。
        /// </summary>
        public int MaxAge { get; set; } = 60;
    }

    public static class CacheOptionsExtensions
    {
        public static TimeSpan ToTimeSpan(this AggregateRootCacheOptions options) 
            => new TimeSpan(0, 0, 0, options.MaxAge);
    }
}
