using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 转款交易已验证的领域事件
    /// </summary>
    public class TransferTransactionValidatedDomainEvent : DomainEvent<BankAccountId>
    {
        public TransferTransactionValidatedDomainEvent(TransferTransactionId transactionId, Money money, TransferAccountType accountType)
        {
            TransactionId = transactionId;
            Money = money;
            AccountType = accountType;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public TransferTransactionId TransactionId { get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Money { get; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferAccountType AccountType { get; }
    }
}
