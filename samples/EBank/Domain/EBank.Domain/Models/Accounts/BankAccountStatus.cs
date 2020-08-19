using System;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 账户状态
    /// </summary>
    [Flags]
    public enum BankAccountStatus
    {
        /// <summary>
        /// 已激活
        /// </summary>
        Activated = 1 << 0,

        /// <summary>
        /// 已失活
        /// </summary>
        Deactivated = 1 << 1
    }
}
