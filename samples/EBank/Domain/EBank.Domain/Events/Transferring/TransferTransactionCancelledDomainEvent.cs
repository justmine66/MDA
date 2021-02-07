using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易已取消的领域事件
    /// </summary>
    public class TransferTransactionCancelledDomainEvent : EndSubTransactionDomainEvent<TransferTransactionId>
    {
        public TransferTransactionCancelledDomainEvent(TransferTransactionStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public TransferTransactionStatus Status { get; }
    }
}
