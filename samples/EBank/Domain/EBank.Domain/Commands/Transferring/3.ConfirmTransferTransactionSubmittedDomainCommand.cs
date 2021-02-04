using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 确认转账交易已提交的领域命令
    /// </summary>
    public class ConfirmTransferTransactionSubmittedDomainCommand : DomainCommand<TransferTransaction, TransferTransactionId> { }
}
