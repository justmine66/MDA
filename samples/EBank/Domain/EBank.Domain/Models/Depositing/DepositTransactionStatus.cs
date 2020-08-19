﻿using System;

namespace EBank.Domain.Models.Depositing
{
    /// <summary>
    /// 存款交易状态
    /// </summary>
    [Flags]
    public enum DepositTransactionStatus
    {
        /// <summary>
        /// 交易已发起
        /// </summary>
        Started = 1 << 0,

        /// <summary>
        /// 交易已验证
        /// </summary>
        Validated = 1 << 1,

        /// <summary>
        /// 交易已完成
        /// </summary>
        Completed = 1 << 2,

        /// <summary>
        /// 交易已取消
        /// </summary>
        Canceled = 1 << 3
    }
}
