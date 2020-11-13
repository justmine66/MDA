using System;

namespace MDA.Domain.Models
{
    [Flags]
    public enum AggregateRootCheckpointStatus
    {
        /// <summary>
        /// 已成功
        /// </summary>
        Succeed = 1 << 0,

        /// <summary>
        /// 已失败
        /// </summary>
        Failed = 1 << 1,

        /// <summary>
        /// 已超时
        /// </summary>
        TimeOuted = 1 << 2,
    }
}
