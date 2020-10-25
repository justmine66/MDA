using EBank.Domain.Models.Transferring;
using MDA.Domain.Commands;
using TransferAccountType = EBank.Domain.Models.Transferring.TransferAccountType;

namespace EBank.Domain.Commands.Transferring
{
    public class ConfirmTransferTransactionValidatedDomainCommand : DomainCommand<TransferTransaction, long>
    {
        public ConfirmTransferTransactionValidatedDomainCommand(long transactionId, TransferAccountType accountType)
        {
            AggregateRootId = transactionId;
            AccountType = accountType;
        }

        public TransferAccountType AccountType { get; private set; }
    }
}
