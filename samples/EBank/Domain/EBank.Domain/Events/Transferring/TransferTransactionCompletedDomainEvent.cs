using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易已完成的领域事件
    /// </summary>
    public class TransferTransactionCompletedDomainEvent : DomainEvent<TransferTransactionId>
    {
        public TransferTransactionCompletedDomainEvent(TransferTransactionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; }
    }
}
