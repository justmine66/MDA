using EBank.Domain.Commands.Depositing;
using EBank.Domain.Events.Depositing;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Models;

namespace EBank.Domain.Models.Depositing
{
    /// <summary>
    /// 表示一笔存款交易，内存状态。
    /// 定义：将金额存入哪个银行的哪个账户。
    /// </summary>
    public partial class DepositTransaction
    {
        /// <summary>
        /// 账户号
        /// </summary>
        public BankAccountId AccountId { get; private set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; private set; }

        /// <summary>
        /// 金额
        /// </summary>
        public Money Money { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DepositTransactionStatus Status { get; private set; }
    }

    /// <summary>
    /// 表示一笔存款交易，处理业务。
    /// </summary>
    public partial class DepositTransaction : EventSourcedAggregateRoot<DepositTransactionId>
    {
        #region [ Handler Domain Commands ]

        public void OnDomainCommand(StartDepositTransactionDomainCommand command)
        {
            var @event = new DepositTransactionStartedDomainEvent(command.AccountId, command.AccountName, command.Bank, command.Money);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmDepositTransactionValidatedDomainCommand command)
        {
            var @event = new DepositTransactionReadiedDomainEvent(AccountId, DepositTransactionStatus.Validated);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmDepositTransactionSubmittedDomainCommand command)
        {
            var @event = new DepositTransactionCompletedDomainEvent(DepositTransactionStatus.Completed, $"当前账户信息，余额：{command.AccountBalance.ToShortString()}, 在途收入余额：{command.AccountInAmountInFlight.ToShortString()}, 在途支出余额：{command.AccountOutAmountInFlight.ToShortString()}");

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(CancelDepositTransactionDomainCommand command)
        {
            var @event = new DepositTransactionCancelledDomainEvent(DepositTransactionStatus.Canceled, command.Message);

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(DepositTransactionStartedDomainEvent @event)
        {
            AccountId = @event.AccountId;
            AccountName = @event.AccountName;
            Bank = @event.Bank;
            Money = @event.Money;
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
