using EBank.Domain.Models.Transferring;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 启动转账交易的领域命令
    /// </summary>
    public class StartTransferTransactionDomainCommand : DomainCommand<TransferTransaction, long>
    {
        public StartTransferTransactionDomainCommand(
            long transactionId,
            TransferTransactionAccount sourceAccount, 
            TransferTransactionAccount sinkAccount, 
            decimal amount)
        {
            AggregateRootId = transactionId;
            SourceAccount = sourceAccount;
            SinkAccount = sinkAccount;
            Amount = amount;
        }

        /// <summary>
        /// 源账户信息
        /// </summary>
        public TransferTransactionAccount SourceAccount { get; }

        /// <summary>
        /// 目标账户信息
        /// </summary>
        public TransferTransactionAccount SinkAccount { get; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Amount { get; }
    }
}
