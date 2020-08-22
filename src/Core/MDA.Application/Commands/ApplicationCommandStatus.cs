using System;

namespace MDA.Application.Commands
{
    [Flags]
    public enum ApplicationCommandStatus
    {
        /// <summary>
        /// 已成功
        /// </summary>
        Successed = 1 << 0,

        /// <summary>
        /// 已失败
        /// </summary>
        Failed = 1 << 1,

        /// <summary>
        /// 已超时
        /// </summary>
        Timeouted = 1 << 2,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 1 << 3,
    }
}
