using EBank.Domain.Models.Depositing;
using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易已取消的领域事件
    /// </summary>
    public class DepositTransactionCancelledDomainEvent : EndSubTransactionDomainEvent<DepositTransactionId>
    {
        public DepositTransactionCancelledDomainEvent(DepositTransactionStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status { get; }
    }
}