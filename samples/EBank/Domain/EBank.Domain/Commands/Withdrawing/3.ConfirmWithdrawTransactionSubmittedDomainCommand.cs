using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 确认取款交易已提交的领域命令
    /// </summary>
    public class ConfirmWithdrawTransactionSubmittedDomainCommand : EndSubTransactionDomainCommand<WithdrawTransaction, WithdrawTransactionId>
    {
        public ConfirmWithdrawTransactionSubmittedDomainCommand(
            long transactionId,
            Money accountBalance,
            Money accountInAmountInFlight,
            Money accountOutAmountInFlight)
        {
            AggregateRootId = transactionId;
            AccountBalance = accountBalance;
            AccountInAmountInFlight = accountInAmountInFlight;
            AccountOutAmountInFlight = accountOutAmountInFlight;
        }

        /// <summary>
        /// 账户余额
        /// </summary>
        public Money AccountBalance { get; }

        /// <summary>
        /// 账户在途收入金额
        /// </summary>
        public Money AccountInAmountInFlight { get; }

        /// <summary>
        /// 账户在途支出金额
        /// </summary>
        public Money AccountOutAmountInFlight { get; }
    }
}