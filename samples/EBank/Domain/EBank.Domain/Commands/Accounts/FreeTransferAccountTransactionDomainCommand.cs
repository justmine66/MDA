using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 释放在途转账账户交易的领域命令
    /// </summary>
    public class FreeTransferAccountTransactionDomainCommand : SubTransactionDomainCommand<BankAccount, BankAccountId>
    {
        public FreeTransferAccountTransactionDomainCommand(BankAccountId accountId, TransferTransactionId transactionId)
        {
            AggregateRootId = accountId;
            TransactionId = transactionId;
        }

        public TransferTransactionId TransactionId { get; }
    }
}