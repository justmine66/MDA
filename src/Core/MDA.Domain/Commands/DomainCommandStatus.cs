using System;

namespace MDA.Domain.Commands
{
    [Flags]
    public enum DomainCommandStatus
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
