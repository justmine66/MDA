using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 启动转账交易的领域命令
    /// </summary>
    public class StartTransferTransactionDomainCommand : DomainCommand<TransferTransaction, TransferTransactionId>
    {
        public StartTransferTransactionDomainCommand(
            TransferTransactionId transactionId,
            TransferAccount sourceAccount, 
            TransferAccount sinkAccount, 
            Money amount)
        {
            AggregateRootId = transactionId;
            SourceAccount = sourceAccount;
            SinkAccount = sinkAccount;
            Amount = amount;
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
    }
}
