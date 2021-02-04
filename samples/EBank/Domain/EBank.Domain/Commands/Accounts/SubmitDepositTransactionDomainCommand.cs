using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 提交存款交易的领域命令
    /// </summary>
    public class SubmitDepositTransactionDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        public SubmitDepositTransactionDomainCommand(
            DepositTransactionId transactionId,
            BankAccountId accountId)
        {
            AggregateRootId = accountId;
            TransactionId = transactionId;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public DepositTransactionId TransactionId { get; }
    }
}
