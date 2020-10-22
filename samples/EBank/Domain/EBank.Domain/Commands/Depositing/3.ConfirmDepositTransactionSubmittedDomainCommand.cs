using EBank.Domain.Models.Depositing;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 确认存款交易已提交的领域命令
    /// </summary>
    public class ConfirmDepositTransactionSubmittedDomainCommand : DomainCommand<DepositTransaction, long> { }
}
