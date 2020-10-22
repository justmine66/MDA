using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 取款交易已验证的领域事件
    /// </summary>
    public class WithdrawTransactionValidatedDomainEvent : DomainEvent<long>
    {
        public WithdrawTransactionValidatedDomainEvent(long transactionId, decimal amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get;  }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get; }
    }
}
