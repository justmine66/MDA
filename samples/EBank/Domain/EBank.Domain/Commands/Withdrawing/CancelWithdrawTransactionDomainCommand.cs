using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 取消取款交易的领域命令
    /// </summary>
    public class CancelWithdrawTransactionDomainCommand : SubTransactionDomainCommand<WithdrawTransaction, WithdrawTransactionId>
    {
        public string Message { get; set; }
    }
}
