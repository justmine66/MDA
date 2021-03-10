using EBank.Domain.Models.Accounts.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 账户名已改变的领域事件
    /// </summary>
    public class AccountNameChangedDomainEvent : DomainEvent<BankAccountId>
    {
        public AccountNameChangedDomainEvent(BankAccountName accountName)
        {
            AccountName = accountName;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; }
    }
}
