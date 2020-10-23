using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;

namespace EBank.Domain.Events.Transferring
{
    public class TransferTransactionStartedDomainEvent : DomainEvent<long>
    {
        public TransferTransactionStartedDomainEvent(
            TransferAccountInfo sourceAccount, 
            TransferAccountInfo sinkAccount, 
            decimal amount, 
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
        public TransferAccountInfo SourceAccount { get; private set; }

        /// <summary>
        /// 目标账户信息
        /// </summary>
        public TransferAccountInfo SinkAccount { get; private set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; private set; }
    }
}
