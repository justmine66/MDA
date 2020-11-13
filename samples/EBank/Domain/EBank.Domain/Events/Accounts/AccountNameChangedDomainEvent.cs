using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 账户名已改变的领域事件
    /// </summary>
    public class AccountNameChangedDomainEvent : DomainEvent<long>
    {
        public AccountNameChangedDomainEvent(string accountName)
        {
            AccountName = accountName;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; }
    }
}
