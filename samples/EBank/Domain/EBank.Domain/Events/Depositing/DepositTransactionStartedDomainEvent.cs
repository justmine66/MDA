using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    public class DepositTransactionStartedDomainEvent : DomainEvent<long>
    {
        public DepositTransactionStartedDomainEvent(
            long accountId,
            string accountName,
            string bank,
            decimal amount)
        {
            AccountId = accountId;
            AccountName = accountName;
            Bank = bank;
            Amount = amount;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; private set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; private set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; private set; }
    }
}
