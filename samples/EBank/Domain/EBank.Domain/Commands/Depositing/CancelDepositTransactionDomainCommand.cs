using EBank.Domain.Models.Depositing;
using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 取消存款交易的领域命令
    /// </summary>
    public class CancelDepositTransactionDomainCommand : SubTransactionDomainCommand<DepositTransaction, DepositTransactionId> { }
}
