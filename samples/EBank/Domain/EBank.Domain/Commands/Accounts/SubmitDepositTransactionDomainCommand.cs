using EBank.Domain.Models.Accounts;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 提交存款交易的领域命令
    /// </summary>
    public class SubmitDepositTransactionDomainCommand : DomainCommand<BankAccount, long>
    {
        public SubmitDepositTransactionDomainCommand(
            long transactionId, 
            long accountId)
        {
            AggregateRootId = accountId;
            TransactionId = transactionId;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; }
    }
}
