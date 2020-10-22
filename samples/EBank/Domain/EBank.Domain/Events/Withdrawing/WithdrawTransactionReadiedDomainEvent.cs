using EBank.Domain.Models.Withdrawing;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已准备就绪的领域事件
    /// </summary>
    public class WithdrawTransactionReadiedDomainEvent : DomainEvent<long>
    {
        public WithdrawTransactionReadiedDomainEvent(
            long accountId, 
            WithdrawTransactionStatus status)
        {
            AccountId = accountId;
            Status = status;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WithdrawTransactionStatus Status { get; private set; }
    }
}
