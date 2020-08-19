using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 取消取款交易的领域命令
    /// </summary>
    public class CancelWithdrawTransactionDomainCommand : DomainCommand<long> { }
}
