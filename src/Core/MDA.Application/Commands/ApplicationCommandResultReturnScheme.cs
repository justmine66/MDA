using System;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令执行结果的返回方案
    /// </summary>
    [Flags]
    public enum ApplicationCommandResultReturnScheme
    {
        /// <summary>
        /// 当应用层命令被处理后，返回执行结果。
        /// </summary>

        OnApplicationCommandHandled = 1 << 0,

        /// <summary>
        /// 当领域命令被处理后，返回执行结果。
        /// </summary>

        OnDomainCommandHandled = 1 << 1,

        /// <summary>
        /// 当领域事件被处理后，返回执行结果。
        /// </summary>

        OnDomainEventHandled = 1 << 2,
    }
}
