using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易已完成的领域事件
    /// </summary>
    public class TransferTransactionCompletedDomainEvent : EndSubTransactionDomainEvent<TransferTransactionId>
    {
        public TransferTransactionCompletedDomainEvent(TransferTransactionStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; }
    }
}
