using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 取款账户交易已提交的领域事件
    /// </summary>
    public class WithdrawTransactionSubmittedDomainEvent : DomainEvent<BankAccountId>
    {
        public WithdrawTransactionSubmittedDomainEvent(WithdrawTransactionId transactionId, Money amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get; }

        /// <summary>
        /// 金额
        /// </summary>
        public Money Amount { get; }
    }
}
