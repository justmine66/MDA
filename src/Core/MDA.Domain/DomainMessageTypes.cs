using System;

namespace MDA.Domain
{
    /// <summary>
    /// 领域消息类型
    /// </summary>
    [Flags]
    public enum DomainMessageTypes
    {
        /// <summary>
        /// 命令
        /// </summary>
        Command = 1 << 0,

        /// <summary>
        /// 事件
        /// </summary>
        Event = 1 << 1,

        /// <summary>
        /// 通知
        /// </summary>
        Notification = 1 << 2,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 1 << 3
    }
}
