using System;

namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 转账交易状态
    /// </summary>
    [Flags]
    public enum TransferTransactionStatus
    {
        /// <summary>
        /// 已发起
        /// </summary>
        Started = 1 << 0,

        /// <summary>
        /// 已验证
        /// </summary>
        Validated = 1 << 1,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 1 << 1,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 1 << 2,
    }
}
