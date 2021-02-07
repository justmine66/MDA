using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 存款账户交易已提交的领域事件
    /// </summary>
    public class DepositTransactionSubmittedDomainEvent : EndSubTransactionDomainEvent<BankAccountId>
    {
        public DepositTransactionSubmittedDomainEvent(DepositTransactionId transactionId, 
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
        public DepositTransactionId TransactionId { get; }

        /// <summary>
        /// 存款金额
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
        /// 账户在途支出金额
        /// </summary>
        public Money AccountOutAmountInFlight { get; }
    }
}
