using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    public class DepositTransactionStartedDomainEvent : DomainEvent<DepositTransactionId>
    {
        public DepositTransactionStartedDomainEvent(
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
