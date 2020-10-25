using MDA.Domain.Events;
using TransferAccountType = EBank.Domain.Models.Transferring.TransferAccountType;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 转账交易已提交的领域事件
    /// </summary>
    public class TransferTransactionSubmittedDomainEvent : DomainEvent<long>
    {
        public TransferTransactionSubmittedDomainEvent(
            long transactionId, 
            decimal amount, 
            TransferAccountType accountType)
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
        public TransferAccountType AccountType { get; }
    }
}
