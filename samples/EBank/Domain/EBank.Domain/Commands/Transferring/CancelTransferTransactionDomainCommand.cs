using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 取消转账交易的领域命令
    /// </summary>
    public class CancelTransferTransactionDomainCommand : DomainCommand<long> { }
}
