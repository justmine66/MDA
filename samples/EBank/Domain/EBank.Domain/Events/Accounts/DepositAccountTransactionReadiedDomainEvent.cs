using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 存款账户交易已准备就绪的领域事件
    /// </summary>
    public class DepositAccountTransactionReadiedDomainEvent : DomainEvent<long>
    {
        public DepositAccountTransactionReadiedDomainEvent(
            long transactionId,
            decimal amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; private set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public decimal Amount { get; private set; }
    }
}
