using System;

namespace MDA.Domain.Commands
{
    [Flags]
    public enum DomainCommandStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0 << 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 0 << 1,

        /// <summary>
        /// 超时
        /// </summary>
        TimeOuted = 0 << 2,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 0 << 3,

        /// <summary>
        /// 已发布
        /// </summary>
        Published = 0 << 4
    }
}
