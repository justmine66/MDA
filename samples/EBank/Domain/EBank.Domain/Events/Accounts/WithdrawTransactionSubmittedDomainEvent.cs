using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 取款账户交易已提交的领域事件
    /// </summary>
    public class WithdrawTransactionSubmittedDomainEvent : EndSubTransactionDomainEvent<BankAccountId>
    {
        public WithdrawTransactionSubmittedDomainEvent(
            WithdrawTransactionId transactionId, 
            Money money, 
            Money accountBalance,
            Money accountInAmountInFlight,
            Money accountOutAmountInFlight)
        {
            TransactionId = transactionId;
            Money = money;
            AccountBalance = accountBalance;
            AccountInAmountInFlight = accountInAmountInFlight;
            AccountOutAmountInFlight = accountOutAmountInFlight;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get; }

        /// <summary>
        /// 金额
        /// </summary>
        public Money Money { get; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public Money AccountBalance { get; }

        /// <summary>
        /// 账户在途收入金额
        /// </summary>
        public Money AccountInAmountInFlight { get; }

        /// <summary>
        /// 账户在途支出总额
        /// </summary>
        public Money AccountOutAmountInFlight { get; }
    }
}
