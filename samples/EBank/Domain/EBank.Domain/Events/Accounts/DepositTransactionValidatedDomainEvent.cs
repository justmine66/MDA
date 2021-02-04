using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 存款交易已验证的领域事件
    /// </summary>
    public class DepositTransactionValidatedDomainEvent : DomainEvent<BankAccountId>
    {
        public DepositTransactionValidatedDomainEvent(DepositTransactionId transactionId, Money amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public DepositTransactionId TransactionId { get; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Amount { get; }
    }
}
