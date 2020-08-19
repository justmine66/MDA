using EBank.Domain.Commands.Withdrawing;
using EBank.Domain.Events.Withdrawing;
using MDA.Domain.Models;
using MDA.Domain.Shared;

namespace EBank.Domain.Models.Withdrawing
{
    /// <summary>
    /// 表示一笔取款交易
    /// 定义：从哪个银行的哪个账户取款。
    /// </summary>
    public partial class WithdrawTransaction : EventSourcedAggregateRoot<long>
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

    public partial class WithdrawTransaction
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

        public void OnDomainCommand(ConfirmWithdrawTransactionValidatePassedDomainCommand command)
        {
            var @event = new WithdrawTransactionValidateCompletedDomainEvent(AccountId, AccountName, Bank, Amount);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmWithdrawTransactionCompletedDomainCommand command)
        {
            var @event = new WithdrawTransactionCompletedDomainEvent();

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(CancelWithdrawTransactionDomainCommand command)
        {
            var @event = new WithdrawTransactionCancelledDomainEvent();

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(WithdrawTransactionStartedDomainEvent @event)
        {
            var account = new WithdrawTransaction(@event.AggregateRootId, @event.AccountId, @event.AccountName, @event.Bank, @event.Amount);
        }

        public void OnDomainEvent(WithdrawTransactionValidateCompletedDomainEvent @event)
        {
            Status = WithdrawTransactionStatus.Validated;
        }

        public void OnDomainEvent(WithdrawTransactionCompletedDomainEvent @event)
        {
            Status = WithdrawTransactionStatus.Completed;
        }

        public void OnDomainEvent(WithdrawTransactionCancelledDomainEvent @event)
        {
            Status = WithdrawTransactionStatus.Canceled;
        }

        #endregion
    }
}
