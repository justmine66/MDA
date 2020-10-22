using EBank.Domain.Models.Depositing;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易信息验证完成的领域事件
    /// </summary>
    public class DepositTransactionReadiedDomainEvent : DomainEvent<long>
    {
        public DepositTransactionReadiedDomainEvent(
            long accountId, 
            DepositTransactionStatus status)
        {
            AccountId = accountId;
            Status = status;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get;  }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status{ get;  }
}
}
