using EBank.Domain.Commands.Transferring;
using EBank.Domain.Events.Transferring;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring.Primitives;
using MDA.Domain.Models;
using MDA.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 表示一笔转账交易，内存状态。
    /// </summary>
    public partial class TransferTransaction
    {
        /// <summary>
        /// 源账户信息
        /// </summary>
        public TransferAccount SourceAccount { get; private set; }

        /// <summary>
        /// 目标账户信息
        /// </summary>
        public TransferAccount SinkAccount { get; private set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public Money Money { get; private set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransferTransactionStatus Status { get; private set; }

        /// <summary>
        /// 已验证
        /// </summary>
        public bool Validated => SourceAccount.Validated && SinkAccount.Validated;
    }

    /// <summary>
    /// 表示一笔转账交易，处理业务
    /// </summary>
    public partial class TransferTransaction : EventSourcedAggregateRoot<TransferTransactionId>
    {
        #region [ Handler Domain Commands ]

        public void OnDomainCommand(StartTransferTransactionDomainCommand command)
        {
            var @event = new TransferTransactionStartedDomainEvent(command.SourceAccount, command.SinkAccount, command.Money, TransferTransactionStatus.Started);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(AggregateRootMessagingContext context, ConfirmTransferTransactionValidatedDomainCommand command)
        {
            var accountType = command.AccountType;
            var logger = context.ServiceProvider.GetService<ILogger<TransferTransaction>>();

            if (Status != TransferTransactionStatus.Started)
            {
                var accountTypeString = accountType == TransferAccountType.Source ? "源" : "目标";

                logger.LogWarning($"收到确认转账交易{accountTypeString}账户已验证通过的领域命令，但交易: {Id}, 状态非法: {Status}。");

                return;
            }

            switch (accountType)
            {
                case TransferAccountType.Source:
                    ApplyDomainEvent(new TransferTransactionSourceAccountValidatedDomainEvent());
                    break;
                case TransferAccountType.Sink:
                    ApplyDomainEvent(new TransferTransactionSinkAccountValidatedDomainEvent());
                    break;
            }

            if (!Validated) return;

            ApplyDomainEvent(new TransferTransactionReadiedDomainEvent(SourceAccount.Id, SinkAccount.Id, TransferTransactionStatus.Validated));
        }

        public void OnDomainCommand(ConfirmTransferTransactionSubmittedDomainCommand command)
        {
            var @event = new TransferTransactionCompletedDomainEvent(TransferTransactionStatus.Completed, command.AccountType, $"当前账户信息, 余额: {command.AccountBalance.ToShortString()}, 在途收入总额：{command.AccountInAmountInFlight.ToShortString()}, 在途支出总额：{command.AccountOutAmountInFlight.ToShortString()}");

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(CancelTransferTransactionDomainCommand command)
        {
            var @event = new TransferTransactionCancelledDomainEvent(SourceAccount, SinkAccount, TransferTransactionStatus.Canceled, command.Message);

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(TransferTransactionStartedDomainEvent @event)
        {
            Id = @event.AggregateRootId;
            SourceAccount = @event.SourceAccount;
            SinkAccount = @event.SinkAccount;
            Money = @event.Money;
            Status = @event.Status;
        }

        public void OnDomainEvent(TransferTransactionSourceAccountValidatedDomainEvent @event)
            => SourceAccount.SetValidated();

        public void OnDomainEvent(TransferTransactionSinkAccountValidatedDomainEvent @event)
            => SinkAccount.SetValidated();

        public void OnDomainEvent(TransferTransactionReadiedDomainEvent @event)
            => Status = @event.Status;

        public void OnDomainEvent(TransferTransactionCompletedDomainEvent @event)
            => Status = @event.Status;

        public void OnDomainEvent(TransferTransactionCancelledDomainEvent @event)
            => Status = @event.Status;

        #endregion
    }
}
