using EBank.Domain.Models.Depositing;
using EBank.Domain.Models.Depositing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 确认存款交易已提交的领域命令
    /// </summary>
    public class ConfirmDepositTransactionSubmittedDomainCommand : EndSubTransactionDomainCommand<DepositTransaction, DepositTransactionId> { }
}
