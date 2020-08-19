using MDA.Domain.Events;

namespace EBank.Domain.Events.Accounts
{
    /// <summary>
    /// 已开户的领域事件
    /// </summary>
    public class AccountOpenedDomainEvent : DomainEvent<long>
    {
        public AccountOpenedDomainEvent(
            long accountId,
            string accountName,
            string bank,
            decimal initialBalance)
        {
            AccountName = accountName;
            Bank = bank;
            InitialBalance = initialBalance;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; private set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public decimal InitialBalance { get; private set; }
    }
}
