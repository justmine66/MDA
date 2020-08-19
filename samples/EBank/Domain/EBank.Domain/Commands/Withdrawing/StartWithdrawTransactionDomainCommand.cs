using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 发起取款交易的领域命令
    /// </summary>
    public class StartWithdrawTransactionDomainCommand : DomainCommand<long>
    {
        public StartWithdrawTransactionDomainCommand(
            long accountId, 
            string accountName, 
            decimal amount, 
            string bank)
        {
            AccountId = accountId;
            AccountName = accountName;
            Amount = amount;
            Bank = bank;
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
