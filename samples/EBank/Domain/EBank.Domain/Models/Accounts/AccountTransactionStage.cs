using System;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 账户交易阶段
    /// 对于涉及到多个账户的交易，需要准备阶段先预扣资源，保障多个账户都能成功。
    /// </summary>
    [Flags]
    public enum AccountTransactionStage
    {
        /// <summary>
        /// 准备
        /// 该阶段先预扣资源。
        /// </summary>
        Preparation = 1 << 0,

        /// <summary>
        /// 提交
        /// 该阶段直接结算。
        /// </summary>
        Commitment = 1 << 1
    }
}
