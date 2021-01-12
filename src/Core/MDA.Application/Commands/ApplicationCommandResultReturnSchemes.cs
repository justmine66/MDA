using System;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令执行结果的返回方案
    /// </summary>
    [Flags]
    public enum ApplicationCommandResultReturnSchemes
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
