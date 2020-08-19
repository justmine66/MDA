using MDA.Domain.Models;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 表示一笔账户交易，比如：扣款、存款等。
    /// </summary>
    public class AccountTransaction : Entity<long>
    {
        public AccountTransaction(
            long transactionId,
            decimal transactionAmount,
            AccountTransactionType transactionType,
            AccountTransactionStage transactionStage) : base(transactionId)
        {
            Amount = transactionAmount;
            Type = transactionType;
            Stage = transactionStage;
        }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public AccountTransactionType Type { get; private set; }

        /// <summary>
        /// 交易阶段
        /// </summary>
        public AccountTransactionStage Stage { get; private set; }
    }
}
