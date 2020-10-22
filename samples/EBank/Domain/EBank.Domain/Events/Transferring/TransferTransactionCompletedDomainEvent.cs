using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    public class TransferTransactionCompletedDomainEvent : DomainEvent<long>
    {
        public TransferTransactionCompletedDomainEvent(TransferTransactionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; private set; }
    }
}
