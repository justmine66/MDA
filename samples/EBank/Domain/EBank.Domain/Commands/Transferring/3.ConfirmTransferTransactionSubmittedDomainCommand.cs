using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Transferring
{
    /// <summary>
    /// 确认转账交易已提交的领域命令
    /// </summary>
    public class ConfirmTransferTransactionSubmittedDomainCommand : DomainCommand<TransferTransaction, TransferTransactionId>
    {
        public ConfirmTransferTransactionSubmittedDomainCommand(
            long transactionId,
            TransferAccountType accountType,
            Money accountBalance,
            Money accountInAmountInFlight,
            Money accountOutAmountInFlight)
        {
            AggregateRootId = transactionId;
            AccountType = accountType;
            AccountBalance = accountBalance;
            AccountInAmountInFlight = accountInAmountInFlight;
            AccountOutAmountInFlight = accountOutAmountInFlight;
        }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferAccountType AccountType { get; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public Money AccountBalance { get; }

        /// <summary>
        /// 账户在途收入金额
        /// </summary>
        public Money AccountInAmountInFlight { get; }

        /// <summary>
        /// 账户在途支出总额
        /// </summary>
        public Money AccountOutAmountInFlight { get; }
    }
}