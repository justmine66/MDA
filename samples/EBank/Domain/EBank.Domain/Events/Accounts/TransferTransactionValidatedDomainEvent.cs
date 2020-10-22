using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 转款交易已验证的领域事件
    /// </summary>
    public class TransferTransactionValidatedDomainEvent : DomainEvent<long>
    {
        public TransferTransactionValidatedDomainEvent(long transactionId, decimal amount, TransferTransactionAccountType accountType)
        {
            TransactionId = transactionId;
            Amount = amount;
            AccountType = accountType;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferTransactionAccountType AccountType { get; }
    }
}
