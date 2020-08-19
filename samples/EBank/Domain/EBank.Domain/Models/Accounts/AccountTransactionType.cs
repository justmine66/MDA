using System;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 账户交易类型
    /// 从账户金额变动的角度看，只有两种交易：取款、存款。
    /// </summary>
    [Flags]
    public enum AccountTransactionType
    {
        /// <summary>
        /// 取款
        /// </summary>
        Withdraw = 1 << 0,

        /// <summary>
        /// 存款
        /// </summary>
        Deposit = 1 << 1,
    }
}
