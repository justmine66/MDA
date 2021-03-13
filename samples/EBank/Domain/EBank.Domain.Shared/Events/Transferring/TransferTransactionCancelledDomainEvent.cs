using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Events.Transferring
{
    /// <summary>
    /// 转账交易已取消的领域事件
    /// </summary>
    public class TransferTransactionCancelledDomainEvent : EndSubTransactionDomainEvent<TransferTransactionId>
    {
        public TransferTransactionCancelledDomainEvent(TransferAccount sourceAccount, TransferAccount sinkAccount, TransferTransactionStatus status, string message)
        {
            SourceAccount = sourceAccount;
            SinkAccount = sinkAccount;
            Message = message;
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

        public TransferTransactionStatus Status { get; }
    }
}
