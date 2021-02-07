using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已取消的领域事件
    /// </summary>
    public class WithdrawTransactionCancelledDomainEvent : EndSubTransactionDomainEvent<WithdrawTransactionId>
    {
        public WithdrawTransactionCancelledDomainEvent(WithdrawTransactionStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public WithdrawTransactionStatus Status { get; }
    }
}
