using EBank.Domain.Commands.Accounts;
using EBank.Domain.Commands.Transferring;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Transferring;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Notifications;
using MDA.Domain.Commands;
using MDA.Domain.Events;
using MDA.Domain.Notifications;

namespace EBank.BusinessServer.ProcessorManagers
{
    /// <summary>
    /// 转账交易处理器
    /// 协调整个转账交易流程
    /// </summary>
    public class TransferringTransactionProcessorManager :
        // 1. 从转账交易聚合根，收到交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        IDomainEventHandler<TransferTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到转账交易已验证的领域事件，通知转账交易聚合根确认。
        IDomainEventHandler<TransferTransactionValidatedDomainEvent>,
        // 2.2 从银行账户聚合根，收到转账交易信息验证失败的通知，通知转账交易聚合根，取消交易。
        IDomainNotificationHandler<TransferTransactionValidateFailedDomainNotification>,
        // 3. 从转账交易聚合根，收到交易信息已准备就绪的领域事件，向银行账户提交转账交易。
        IDomainEventHandler<TransferTransactionReadiedDomainEvent>,
        // 4 从银行账户聚合根，收到转账交易已提交的领域事件，通知转账交易聚合根确认。
        IDomainEventHandler<TransferTransactionSubmittedDomainEvent>
    {
        private readonly IDomainCommandPublisher _domainCommandPublisher;

        public TransferringTransactionProcessorManager(IDomainCommandPublisher domainCommandPublisher)
            => _domainCommandPublisher = domainCommandPublisher;

        /// <summary>
        /// 1. 从转账交易聚合根，收到交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        /// </summary>
        /// <param name="event"></param>
        public void Handle(TransferTransactionStartedDomainEvent @event)
        {
            // 1. 验证源账户
            var validateSource = new ValidateTransferTransactionDomainCommand()
            {
                AggregateRootId = @event.SourceAccount.Id,
                TransactionId = @event.AggregateRootId,
                Account = @event.SourceAccount,
                Amount = @event.Amount
            };

            _domainCommandPublisher.Publish(validateSource);

            // 2. 验证目标账户
            var validateSink = new ValidateTransferTransactionDomainCommand()
            {
                AggregateRootId = @event.SinkAccount.Id,
                TransactionId = @event.AggregateRootId,
                Account = @event.SinkAccount,
                Amount = @event.Amount
            };

            _domainCommandPublisher.Publish(validateSink);
        }

        /// <summary>
        /// 2.1 从银行账户聚合根，收到转账交易已验证的领域事件，通知转账交易聚合根确认。
        /// </summary>
        /// <param name="event"></param>
        public void Handle(TransferTransactionValidatedDomainEvent @event)
        {
            var command = new ConfirmTransferTransactionValidatedDomainCommand(@event.TransactionId, @event.AccountType);

            _domainCommandPublisher.Publish(command);
        }

        /// <summary>
        /// 2.2 从银行账户聚合根，收到转账交易信息验证失败的通知，通知转账交易聚合根，取消交易。
        /// </summary>
        /// <param name="notification"></param>
        public void Handle(TransferTransactionValidateFailedDomainNotification notification)
        {
            var command = new CancelTransferTransactionDomainCommand()
            {
                AggregateRootId = notification.TransactionId
            };

            _domainCommandPublisher.Publish(command);
        }

        /// <summary>
        /// 3. 从转账交易聚合根，收到交易信息已准备就绪的领域事件，向银行账户聚合根，向银行账户提交转账交易。
        /// </summary>
        /// <param name="event"></param>
        public void Handle(TransferTransactionReadiedDomainEvent @event)
        {
            // 1. 从源账户取款
            var withdraw = new SubmitTransferTransactionDomainCommand(@event.AggregateRootId, @event.SourceAccountId);

            _domainCommandPublisher.Publish(withdraw);

            // 2. 存到目标账户
            var deposit = new SubmitTransferTransactionDomainCommand(@event.AggregateRootId, @event.SinkAccountId);

            _domainCommandPublisher.Publish(deposit);
        }

        /// <summary>
        /// 4 从银行账户聚合根，收到转账交易已提交的领域事件，通知转账交易聚合根确认。
        /// </summary>
        /// <param name="event"></param>
        public void Handle(TransferTransactionSubmittedDomainEvent @event)
        {
            var command = new ConfirmTransferTransactionSubmittedDomainCommand()
            {
                AggregateRootId = @event.TransactionId
            };

            _domainCommandPublisher.Publish(command);
        }
    }
}