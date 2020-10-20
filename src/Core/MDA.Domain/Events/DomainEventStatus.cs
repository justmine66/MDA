using System;

namespace MDA.Domain.Events
{
    [Flags]
    public enum DomainEventStatus
    {
        /// <summary>
        /// 已成功
        /// </summary>
        Succeed = 0 << 0,

        /// <summary>
        /// 已失败
        /// </summary>
        Failed = 0 << 1,

        /// <summary>
        /// 已超时
        /// </summary>
        TimeOuted = 0 << 2,
    }
}
