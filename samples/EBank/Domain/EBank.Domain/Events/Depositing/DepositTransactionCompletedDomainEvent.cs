using EBank.Domain.Models.Depositing;
using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易已完成的领域事件
    /// </summary>
    public class DepositTransactionCompletedDomainEvent : EndSubTransactionDomainEvent<DepositTransactionId>
    {
        public DepositTransactionCompletedDomainEvent(DepositTransactionStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status{ get;  }
}
}
