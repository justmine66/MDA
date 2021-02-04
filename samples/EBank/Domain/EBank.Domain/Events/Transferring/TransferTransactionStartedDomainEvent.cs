using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易已发起的领域事件
    /// </summary>
    public class TransferTransactionStartedDomainEvent : DomainEvent<TransferTransactionId>
    {
        public TransferTransactionStartedDomainEvent(
            TransferAccount sourceAccount,
            TransferAccount sinkAccount,
            Money amount,
            TransferTransactionStatus status)
        {
            SourceAccount = sourceAccount;
            SinkAccount = sinkAccount;
            Amount = amount;
            Status = status;
        }

        /// <summary>
        /// 源账户信息
        /// </summary>
        public TransferAccount SourceAccount { get; }

        /// <summary>
        /// 目标账户信息
        /// </summary>
        public TransferAccount SinkAccount { get; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public Money Amount { get; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; }
    }
}
