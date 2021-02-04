using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已取消的领域事件
    /// </summary>
    public class WithdrawTransactionCancelledDomainEvent : DomainEvent<WithdrawTransactionId>
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
