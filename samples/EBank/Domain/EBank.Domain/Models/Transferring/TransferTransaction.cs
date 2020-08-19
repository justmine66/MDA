using EBank.Domain.Commands.Transferring;
using EBank.Domain.Events.Transferring;
using MDA.Domain.Models;

namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 表示一笔转账交易，聚合根。
    /// </summary>
    public partial class TransferTransaction : EventSourcedAggregateRoot<long>
    {
        public TransferTransaction(long id,
            TransferTransactionAccount sourceAccount,
            TransferTransactionAccount sinkAccountInfo,
            decimal amount,
            TransferTransactionStatus status)
            : base(id)
        {
            SourceAccount = sourceAccount;
            SinkAccount = sinkAccountInfo;
            Amount = amount;
            Status = status;
        }

        /// <summary>
        /// 源账户信息
        /// </summary>
        public TransferTransactionAccount SourceAccount { get; private set; }

        /// <summary>
        /// 目标账户信息
        /// </summary>
        public TransferTransactionAccount SinkAccount { get; private set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; private set; }

        public bool IsSourceAndSinkAccountValidatePassed =>
            SourceAccount.IsValidationPassed && SinkAccount.IsValidationPassed;
    }

    public partial class TransferTransaction : EventSourcedAggregateRoot<long>
    {
        #region [ Handler Domain Commands ]

        public void OnDomainCommand(StartTransferTransactionDomainCommand command)
        {
            var @event = new TransferTransactionStartedDomainEvent(command.SourceAccount, command.SinkAccount, command.Amount, TransferTransactionStatus.Started);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmTransferTransactionAccountValidatePassedDomainCommand command)
        {
            if (Status != TransferTransactionStatus.Started)
            {
                throw new TransferTransactionDomainException($"收到确认交易账户验证通过的领域命令，但交易状态非法: {Status}。");
            }

            if (IsSourceAndSinkAccountValidatePassed)
            {
                var validateCompleted = new TransferTransactionValidateCompletedDomainEvent(SourceAccount, SinkAccount, Amount);

                ApplyDomainEvent(validateCompleted);
            }

            switch (command.AccountType)
            {
                case TransferTransactionAccountType.Source:
                    var sourceAccountValidatePassed = new TransferTransactionSourceAccountValidatePassedDomainEvent();

                    ApplyDomainEvent(sourceAccountValidatePassed);
                    break;
                case TransferTransactionAccountType.Sink:
                    var sinkAccountValidatePassed = new TransferTransactionSinkAccountValidatePassedDomainEvent();

                    ApplyDomainEvent(sinkAccountValidatePassed);
                    break;
            }
        }

        public void OnDomainCommand(CancelTransferTransactionDomainCommand command)
        {
            var @event = new TransferTransactionCancelledDomainEvent();

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(TransferTransactionStartedDomainEvent @event)
        {
            var account = new TransferTransaction(@event.AggregateRootId, @event.SourceAccount, @event.SinkAccount, @event.Amount, @event.Status);
        }

        public void OnDomainEvent(TransferTransactionSourceAccountValidatePassedDomainEvent @event)
            => SourceAccount.PassValidation();

        public void OnDomainEvent(TransferTransactionSinkAccountValidatePassedDomainEvent @event)
            => SinkAccount.PassValidation();

        public void OnDomainEvent(TransferTransactionValidateCompletedDomainEvent @event)
            => Status = TransferTransactionStatus.Validated;

        public void OnDomainEvent(TransferTransactionCancelledDomainEvent @event)
            => Status = TransferTransactionStatus.Canceled;

        #endregion
    }
}
