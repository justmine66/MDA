using EBank.Domain.Models.Withdrawing;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 确认取款交易信息验证通过的领域命令
    /// </summary>
    public class ConfirmWithdrawTransactionValidatedDomainCommand : DomainCommand<WithdrawTransaction, long> { }
}
