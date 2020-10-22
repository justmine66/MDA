using EBank.Domain.Commands.Depositing;
using EBank.Domain.Events.Depositing;
using MDA.Domain.Models;
using MDA.Domain.Shared;

namespace EBank.Domain.Models.Depositing
{
    /// <summary>
    /// 表示一笔存款交易
    /// 定义：将金额存入哪个银行的哪个账户。
    /// </summary>
    public partial class DepositTransaction : EventSourcedAggregateRoot<long>
    {
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
        public DepositTransactionStatus Status { get; private set; }
    }

    public partial class DepositTransaction
    {
        #region [ Handler Domain Commands ]

        public void OnDomainCommand(StartDepositTransactionDomainCommand command)
        {
            PreConditions.GreaterThan(nameof(command.AccountId), command.AccountId, 0);
            PreConditions.NotNullOrWhiteSpace(nameof(command.AccountName), command.AccountName);
            PreConditions.NotNullOrWhiteSpace(nameof(command.Bank), command.Bank);
            PreConditions.GreaterThan(nameof(command.Amount), command.Amount, 0);

            var @event = new DepositTransactionStartedDomainEvent(command.AccountId, command.AccountName, command.Bank, command.Amount);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmDepositTransactionValidatedDomainCommand command)
        {
            var @event = new DepositTransactionReadiedDomainEvent(AccountId, DepositTransactionStatus.Validated);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmDepositTransactionSubmittedDomainCommand command)
        {
            var @event = new DepositTransactionCompletedDomainEvent(DepositTransactionStatus.Completed);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(CancelDepositTransactionDomainCommand command)
        {
            var @event = new DepositTransactionCancelledDomainEvent(DepositTransactionStatus.Canceled);

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(DepositTransactionStartedDomainEvent @event)
        {
            AccountId = @event.AccountId;
            AccountName = @event.AccountName;
            Bank = @event.Bank;
            Amount = @event.Amount;
        }

        public void OnDomainEvent(DepositTransactionReadiedDomainEvent @event)
        {
            Status = @event.Status;
        }

        public void OnDomainEvent(DepositTransactionCompletedDomainEvent @event)
        {
            Status = @event.Status;
        }

        public void OnDomainEvent(DepositTransactionCancelledDomainEvent @event)
        {
            Status = @event.Status;
        }

        #endregion
    }
}
