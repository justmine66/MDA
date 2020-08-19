using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Depositing
{
    /// <summary>
    /// 确认存款交易已完成的领域命令
    /// </summary>
    public class ConfirmDepositTransactionCompletedDomainCommand : DomainCommand<long> { }
}
