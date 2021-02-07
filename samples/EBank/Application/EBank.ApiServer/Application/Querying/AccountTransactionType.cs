using System;

namespace EBank.ApiServer.Application.Querying
{
    /// <summary>
    /// 账户交易类型
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

        /// <summary>
        /// 转账
        /// </summary>
        Transfer = 1 << 2
    }
}