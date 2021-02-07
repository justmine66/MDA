using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 取消转账交易的领域命令
    /// </summary>
    public class CancelTransferTransactionDomainCommand : SubTransactionDomainCommand<TransferTransaction, TransferTransactionId>
    {
        public CancelTransferTransactionDomainCommand(TransferTransactionId transferTransactionId, string message)
        {
            AggregateRootId = transferTransactionId;
            Message = message;
        }

        public string Message { get; }
    }
}
