using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    public class TransferTransactionReadiedDomainEvent : DomainEvent<TransferTransactionId>
    {
        public TransferTransactionReadiedDomainEvent(BankAccountId sourceAccountId, BankAccountId sinkAccountId, TransferTransactionStatus status)
        {
            SourceAccountId = sourceAccountId;
            SinkAccountId = sinkAccountId;
            Status = status;
        }

        public BankAccountId SourceAccountId { get; }

        public BankAccountId SinkAccountId { get; }

        public TransferTransactionStatus Status { get; }
    }
}
