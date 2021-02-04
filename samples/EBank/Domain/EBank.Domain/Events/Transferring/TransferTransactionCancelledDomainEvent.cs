using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易已取消的领域事件
    /// </summary>
    public class TransferTransactionCancelledDomainEvent : DomainEvent<TransferTransactionId>
    {
        public TransferTransactionCancelledDomainEvent(TransferTransactionStatus status)
        {
            Status = status;
        }

        public TransferTransactionStatus Status { get; }
    }
}
