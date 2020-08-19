using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 取消存款交易的领域命令
    /// </summary>
    public class CancelDepositTransactionDomainCommand : DomainCommand<long> { }
}
