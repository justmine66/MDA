using EBank.Domain.Models.Depositing;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 取消存款交易的领域命令
    /// </summary>
    public class CancelDepositTransactionDomainCommand : EndSubTransactionDomainCommand<DepositTransaction,long> { }
}
