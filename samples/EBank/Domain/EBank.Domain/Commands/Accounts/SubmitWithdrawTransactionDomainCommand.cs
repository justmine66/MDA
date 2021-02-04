using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 提交取款交易的领域命令
    /// </summary>
    public class SubmitWithdrawTransactionDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        public SubmitWithdrawTransactionDomainCommand(
            WithdrawTransactionId transactionId,
            BankAccountId accountId)
        {
            AggregateRootId = accountId;
            TransactionId = transactionId;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get; }
    }
}
