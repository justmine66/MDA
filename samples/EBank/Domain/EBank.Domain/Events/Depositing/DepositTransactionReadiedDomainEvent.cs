using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing;
using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Depositing
{
    /// <summary>
    /// 存款交易信息验证完成的领域事件
    /// </summary>
    public class DepositTransactionReadiedDomainEvent : DomainEvent<DepositTransactionId>
    {
        public DepositTransactionReadiedDomainEvent(
            BankAccountId accountId, 
            DepositTransactionStatus status)
        {
            AccountId = accountId;
            Status = status;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public BankAccountId AccountId { get;  }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status{ get;  }
}
}
