using EBank.Domain.Models.Transferring;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    public class ConfirmTransferTransactionValidatedDomainCommand : DomainCommand<TransferTransaction, long>
    {
        public ConfirmTransferTransactionValidatedDomainCommand(long transactionId, TransferTransactionAccountType accountType)
        {
            AggregateRootId = transactionId;
            AccountType = accountType;
        }

        public TransferTransactionAccountType AccountType { get; private set; }
    }
}
