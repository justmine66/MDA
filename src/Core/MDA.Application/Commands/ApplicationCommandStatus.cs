using System;

namespace MDA.Application.Commands
{
    [Flags]
    public enum ApplicationCommandStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1 << 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1 << 1,

        /// <summary>
        /// 超时
        /// </summary>
        TimeOut = 1 << 2
    }
}
