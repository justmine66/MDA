using System;

namespace MDA.Domain
{
    /// <summary>
    /// 表示领域层定义的状态码，用于协调端到端流程。
    /// </summary>
    [Flags]
    public enum DomainStatusCodes
    {
        /// <summary>
        /// 已接收领域命令
        /// </summary>
        DomainCommandReceived = 1 << 0,

        /// <summary>
        /// 已处理领域命令
        /// </summary>
        DomainCommandHandled = 1 << 1,

        /// <summary>
        /// 已处理领域事件
        /// </summary>
        DomainEventHandled = 1 << 2
    }
}