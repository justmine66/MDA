﻿using EBank.Domain.Commands.Transferring;
using EBank.Domain.Events.Transferring;
using MDA.Domain.Models;

namespace EBank.Domain.Models.Transferring
{
    /// <summary>
    /// 表示一笔转账交易，聚合根。
    /// </summary>
    public partial class TransferTransaction
    {
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

        /// <summary>
        /// 已验证
        /// </summary>
        public bool Validated => SourceAccount.Validated && SinkAccount.Validated;
    }

    public partial class TransferTransaction : EventSourcedAggregateRoot<long>
    {
        #region [ Handler Domain Commands ]

        public void OnDomainCommand(StartTransferTransactionDomainCommand command)
        {
            var @event = new TransferTransactionStartedDomainEvent(command.SourceAccount, command.SinkAccount, command.Amount, TransferTransactionStatus.Started);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(ConfirmTransferTransactionValidatedDomainCommand command)
        {
            if (Status != TransferTransactionStatus.Started)
            {
                throw new TransferTransactionDomainException($"收到确认交易账户已验证的领域命令，但交易状态非法: {Status}。");
            }

            if (Validated)
            {
                ApplyDomainEvent(new TransferTransactionReadiedDomainEvent(SourceAccount.Id, SinkAccount.Id, TransferTransactionStatus.Validated));

                return;
            }

            switch (command.AccountType)
            {
                case TransferTransactionAccountType.Source:
                    ApplyDomainEvent(new TransferTransactionSourceAccountValidatedDomainEvent());
                    break;
                case TransferTransactionAccountType.Sink:
                    ApplyDomainEvent(new TransferTransactionSinkAccountValidatedDomainEvent());
                    break;
            }
        }

        public void OnDomainCommand(ConfirmTransferTransactionSubmittedDomainCommand command)
        {
            var @event = new TransferTransactionCompletedDomainEvent(TransferTransactionStatus.Completed);

            ApplyDomainEvent(@event);
        }

        public void OnDomainCommand(CancelTransferTransactionDomainCommand command)
        {
            var @event = new TransferTransactionCancelledDomainEvent(TransferTransactionStatus.Canceled);

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(TransferTransactionStartedDomainEvent @event)
        {
            Id = @event.AggregateRootId;
            SourceAccount = @event.SourceAccount;
            SinkAccount = @event.SinkAccount;
            Amount = @event.Amount;
            Status = @event.Status;
        }

        public void OnDomainEvent(TransferTransactionSourceAccountValidatedDomainEvent @event)
            => SourceAccount.SetValidated();

        public void OnDomainEvent(TransferTransactionSinkAccountValidatedDomainEvent @event)
            => SinkAccount.SetValidated();

        public void OnDomainEvent(TransferTransactionReadiedDomainEvent @event)
            => Status = @event.Status;

        public void OnDomainEvent(TransferTransactionCancelledDomainEvent @event)
            => Status = @event.Status;

        #endregion
    }
}
