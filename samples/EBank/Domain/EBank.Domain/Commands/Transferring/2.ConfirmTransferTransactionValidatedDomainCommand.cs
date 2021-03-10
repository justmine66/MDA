using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    public class ConfirmTransferTransactionValidatedDomainCommand : DomainCommand<TransferTransaction, TransferTransactionId>
    {
        public ConfirmTransferTransactionValidatedDomainCommand(TransferTransactionId transactionId, TransferAccountType accountType)
        {
            AggregateRootId = transactionId;
            AccountType = accountType;
        }

        public TransferAccountType AccountType { get; }
    }
}
