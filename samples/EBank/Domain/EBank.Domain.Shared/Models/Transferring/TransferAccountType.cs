using System;

namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 交易账户类型
    /// </summary>
    [Flags]
    public enum TransferAccountType
    {
        /// <summary>
        /// 源账号
        /// </summary>
        Source = 1 << 0,

        /// <summary>
        /// 目标账号
        /// </summary>
        Sink = 1 << 1
    }
}
