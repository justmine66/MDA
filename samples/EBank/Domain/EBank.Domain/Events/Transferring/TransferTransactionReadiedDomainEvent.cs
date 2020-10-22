using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    public class TransferTransactionReadiedDomainEvent : DomainEvent<long>
    {
        public TransferTransactionReadiedDomainEvent(long sourceAccountId, long sinkAccountId, TransferTransactionStatus status)
        {
            SourceAccountId = sourceAccountId;
            SinkAccountId = sinkAccountId;
            Status = status;
        }

        public long SourceAccountId { get; }
        public long SinkAccountId { get; }

        public TransferTransactionStatus Status { get; }
    }
}
