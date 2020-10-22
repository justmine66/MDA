using System;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 账户资金流向
    /// </summary>
    [Flags]
    public enum AccountFundDirection
    {
        /// <summary>
        /// 收入
        /// </summary>
        In = 1 << 0,

        /// <summary>
        /// 支出
        /// </summary>
        Out = 1 << 1
    }
}
