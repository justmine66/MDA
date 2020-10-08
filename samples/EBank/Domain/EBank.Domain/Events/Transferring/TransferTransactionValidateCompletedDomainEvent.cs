using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    public class TransferTransactionValidateCompletedDomainEvent : DomainEvent<long, long>
    {
        public TransferTransactionValidateCompletedDomainEvent(
            TransferTransactionAccount sourceAccount, 
            TransferTransactionAccount sinkAccount, 
            decimal amount)
        {
            SourceAccount = sourceAccount;
            SinkAccount = sinkAccount;
            Amount = amount;
        }

        public TransferTransactionAccount SourceAccount { get; private set; }

        public TransferTransactionAccount SinkAccount { get; private set; }

        public decimal Amount { get; private set; }
    }
}
