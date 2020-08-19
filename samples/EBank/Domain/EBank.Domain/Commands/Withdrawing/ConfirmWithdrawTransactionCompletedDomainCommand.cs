using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 确认取款交易已完成的领域命令
    /// </summary>
    public class ConfirmWithdrawTransactionCompletedDomainCommand : DomainCommand<long> { }
}
