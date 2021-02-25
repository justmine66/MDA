using System;

namespace MDA.Domain.Shared
{
    /// <summary>
    /// 应用层命令回复方案
    /// </summary>
    [Flags]
    public enum ApplicationCommandReplySchemes
    {
        /// <summary>
        /// 不返回结果。
        /// </summary>

        None = 1 << 0,

        /// <summary>
        /// 当领域命令被处理后，返回执行结果。
        /// </summary>

        OnDomainCommandHandled = 1 << 2,

        /// <summary>
        /// 当领域事件被处理后，返回执行结果。
        /// </summary>

        OnDomainEventHandled = 1 << 3
    }
}
