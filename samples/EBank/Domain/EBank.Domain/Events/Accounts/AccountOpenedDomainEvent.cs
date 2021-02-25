using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Shared.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 已开户的领域事件
    /// </summary>
    public class AccountOpenedDomainEvent : DomainEvent<BankAccountId>
    {
        public AccountOpenedDomainEvent(
            BankAccountName accountName,
            BankName bank,
            Money initialBalance)
        {
            AccountName = accountName;
            Bank = bank;
            InitialBalance = initialBalance;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public Money InitialBalance { get; }
    }
}
