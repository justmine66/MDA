using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Withdrawing
{
    /// <summary>
    /// 取款交易已准备就绪的领域事件
    /// </summary>
    public class WithdrawTransactionReadiedDomainEvent : SubTransactionDomainEvent<WithdrawTransactionId>
    {
        public WithdrawTransactionReadiedDomainEvent(
            BankAccountId accountId, 
            WithdrawTransactionStatus status)
        {
            AccountId = accountId;
            Status = status;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public BankAccountId AccountId { get; }

        /// <summary>
        /// 状态
        /// </summary>
        public WithdrawTransactionStatus Status { get; }
    }
}
