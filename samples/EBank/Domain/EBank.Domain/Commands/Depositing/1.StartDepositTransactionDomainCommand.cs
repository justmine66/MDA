using EBank.Domain.Models.Depositing;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 发起存款交易的领域命令
    /// </summary>
    public class StartDepositTransactionDomainCommand : BeginSubTransactionDomainCommand<DepositTransaction, long>
    {
        public StartDepositTransactionDomainCommand(
            long transactionId,
            long accountId,
            string accountName,
            decimal amount,
            string bank)
        {
            AggregateRootId = transactionId;
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
