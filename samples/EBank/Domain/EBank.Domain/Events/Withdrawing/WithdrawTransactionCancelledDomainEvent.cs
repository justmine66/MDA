using EBank.Domain.Models.Withdrawing;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已取消的领域事件
    /// </summary>
    public class WithdrawTransactionCancelledDomainEvent : DomainEvent<long>
    {
        public WithdrawTransactionCancelledDomainEvent(WithdrawTransactionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public WithdrawTransactionStatus Status { get; }
    }
}
