using MDA.Domain.Events;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易信息验证完成的领域事件
    /// </summary>
    public class WithdrawTransactionValidateCompletedDomainEvent : DomainEvent<long, long>
    {
        public WithdrawTransactionValidateCompletedDomainEvent(
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
