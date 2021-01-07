using EBank.Domain.Commands.Withdrawing;
using EBank.Domain.Events.Withdrawing;
using MDA.Domain.Models;
using MDA.Domain.Shared;

namespace EBank.Domain.Models.Withdrawing
{
    /// <summary>
    /// 表示一笔取款交易，内存状态。
    /// 定义：从哪个银行的哪个账户取款。
    /// </summary>
    public partial class WithdrawTransaction 
    {
        public WithdrawTransaction(
            long id,
            long accountId,
            string accountName,
            string bank,
            decimal amount) : base(id)
        {
            AccountId = accountId;
            AccountName = accountName;
            Bank = bank;
            Amount = amount;
            Status = WithdrawTransactionStatus.Started;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; private set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; private set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WithdrawTransactionStatus Status { get; private set; }
    }

    /// <summary>
    /// 表示一笔取款交易，处理业务。
    /// </summary>
    public partial class WithdrawTransaction : EventSourcedAggregateRoot<long>
    {
        #region [ Handler Domain Commands ]

        public void OnDomainCommand(StartWithdrawTransactionDomainCommand command)
        {
            PreConditions.GreaterThan(nameof(command.AccountId), command.AccountId, 0);
            PreConditions.NotNullOrWhiteSpace(nameof(command.AccountName), command.AccountName);
            PreConditions.NotNullOrWhiteSpace(nameof(command.Bank), command.Bank);
            PreConditions.GreaterThan(nameof(command.Amount), command.Amount, 0);

            var @event = new WithdrawTransactionStartedDomainEvent(command.AccountId, command.AccountName, command.Bank, command.Amount);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmWithdrawTransactionValidatedDomainCommand command)
        {
            var @event = new WithdrawTransactionReadiedDomainEvent(AccountId, WithdrawTransactionStatus.Validated);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmWithdrawTransactionSubmittedDomainCommand command)
        {
            var @event = new WithdrawTransactionCompletedDomainEvent(WithdrawTransactionStatus.Completed);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(CancelWithdrawTransactionDomainCommand command)
        {
            var @event = new WithdrawTransactionCancelledDomainEvent(WithdrawTransactionStatus.Canceled);

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(WithdrawTransactionStartedDomainEvent @event)
        {
            AccountId = @event.AccountId;
            AccountName = @event.AccountName;
            Bank = @event.Bank;
            Amount = @event.Amount;
        }

        public void OnDomainEvent(WithdrawTransactionReadiedDomainEvent @event)
        {
            Status = @event.Status;
        }

        public void OnDomainEvent(WithdrawTransactionCompletedDomainEvent @event)
        {
            Status = @event.Status;
        }

        public void OnDomainEvent(WithdrawTransactionCancelledDomainEvent @event)
        {
            Status = @event.Status;
        }

        #endregion
    }
}
