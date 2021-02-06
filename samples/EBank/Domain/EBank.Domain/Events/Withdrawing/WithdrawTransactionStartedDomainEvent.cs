using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已发起的领域事件
    /// </summary>
    public class WithdrawTransactionStartedDomainEvent : SubTransactionDomainEvent<WithdrawTransactionId>
    {
        public WithdrawTransactionStartedDomainEvent(
            BankAccountId accountId,
            BankAccountName accountName,
            BankName bank,
            Money money)
        {
            AccountId = accountId;
            AccountName = accountName;
            Bank = bank;
            Money = money;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public BankAccountId AccountId { get; }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; }

        /// <summary>
        /// 金额
        /// </summary>
        public Money Money { get; }
    }
}
