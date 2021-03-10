using System;

namespace MDA.Domain.Commands
{
    [Flags]
    public enum DomainCommandStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 1 << 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1 << 1,

        /// <summary>
        /// 超时
        /// </summary>
        TimeOuted = 1 << 2,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 1 << 3,

        /// <summary>
        /// 已发布
        /// </summary>
        Published = 1 << 4
    }
}
