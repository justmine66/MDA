using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 确认取款交易已提交的领域命令
    /// </summary>
    public class ConfirmWithdrawTransactionSubmittedDomainCommand : DomainCommand<WithdrawTransaction, WithdrawTransactionId> { }
}
