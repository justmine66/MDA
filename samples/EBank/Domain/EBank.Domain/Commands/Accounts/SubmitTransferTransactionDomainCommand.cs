using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Shared.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 提交转账交易的领域命令
    /// </summary>
    public class SubmitTransferTransactionDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        public SubmitTransferTransactionDomainCommand(TransferTransactionId transactionId, BankAccountId accountId)
        {
            AggregateRootId = accountId;
            TransactionId = transactionId;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public TransferTransactionId TransactionId { get; }
    }
}
