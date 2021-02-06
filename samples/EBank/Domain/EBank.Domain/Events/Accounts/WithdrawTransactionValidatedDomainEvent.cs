using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 取款交易已验证的领域事件
    /// </summary>
    public class WithdrawTransactionValidatedDomainEvent : SubTransactionDomainEvent<BankAccountId>
    {
        public WithdrawTransactionValidatedDomainEvent(WithdrawTransactionId transactionId, Money money)
        {
            TransactionId = transactionId;
            Money = money;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get;  }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Money { get; }
    }
}
