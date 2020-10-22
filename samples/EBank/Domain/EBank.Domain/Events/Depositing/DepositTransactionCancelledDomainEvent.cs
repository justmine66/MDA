using EBank.Domain.Models.Depositing;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易已取消的领域事件
    /// </summary>
    public class DepositTransactionCancelledDomainEvent : DomainEvent<long>
    {
        public DepositTransactionCancelledDomainEvent(DepositTransactionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status { get; }
    }
}
