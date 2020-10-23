using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 存款账户交易已提交的领域事件
    /// </summary>
    public class DepositTransactionSubmittedDomainEvent : DomainEvent<long>
    {
        public DepositTransactionSubmittedDomainEvent(long transactionId, decimal amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; }
    }
}
