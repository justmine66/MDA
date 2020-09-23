using System;

namespace MDA.Domain.Models
{
    public class AggregateRootCacheOptions
    {
        /// <summary>
        /// 缓存容量上限，默认：int.MaxValue。
        /// </summary>
        public int MaxSize { get; set; } = int.MaxValue;

        /// <summary>
        /// 缓存条目存活时长(Time To Live)，单位：秒，默认：1分钟，即一分钟内没有被访问，将从缓存中清理掉。
        /// </summary>
        public int TTL { get; set; } = 60;
    }

    public static class CacheOptionsExtensions
    {
        public static TimeSpan ToTimeSpan(this AggregateRootCacheOptions options) => new TimeSpan(0, 0, 0, options.TTL);
    }
}
