using EBank.Domain.Commands.Accounts;
using EBank.Domain.Commands.Transferring;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Transferring;
using EBank.Domain.Notifications.Accounts;
using MDA.Domain.Events;
using MDA.Domain.Notifications;

namespace EBank.BusinessServer.ProcessorManagers
{
    /// <summary>
    /// 转账交易流程管理器
    /// 协调整个转账交易流程
    /// </summary>
    public class TransferringTransactionProcessorManager :
        // 1. 从转账交易聚合根，收到交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        IDomainEventHandler<TransferTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到转账交易已验证的领域通知，通知转账交易聚合根确认。
        IDomainNotificationHandler<TransferTransactionValidatedDomainNotification>,
        // 2.2 从银行账户聚合根，收到转账交易信息验证失败的通知，通知转账交易聚合根，取消交易。
        IDomainNotificationHandler<TransferTransactionValidateFailedDomainNotification>,
        // 2.3 从转账交易聚合根，收到转账交易已取消的领域事件，通知银行账户聚合根释放在途交易。
        IDomainEventHandler<TransferTransactionCancelledDomainEvent>,
        // 3. 从转账交易聚合根，收到交易信息已准备就绪的领域事件，向银行账户提交转账交易。
        IDomainEventHandler<TransferTransactionReadiedDomainEvent>,
        // 4. 从银行账户聚合根，收到转账交易已提交的领域事件，通知转账交易聚合根确认。
        IDomainEventHandler<TransferTransactionSubmittedDomainEvent>
    {
        /// <summary>
        /// 1. 从转账交易聚合根，收到交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">交易已发起的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, TransferTransactionStartedDomainEvent @event)
        {
            // 1. 验证源账户
            var validateSource = new ValidateTransferTransactionDomainCommand()
            {
                AggregateRootId = @event.SourceAccount.Id,
                TransactionId = @event.AggregateRootId,
                Account = @event.SourceAccount,
                Money = @event.Money
            };

            context.PublishDomainCommand(validateSource);

            // 2. 验证目标账户
            var validateSink = new ValidateTransferTransactionDomainCommand()
            {
                AggregateRootId = @event.SinkAccount.Id,
                TransactionId = @event.AggregateRootId,
                Account = @event.SinkAccount,
                Money = @event.Money
            };

            context.PublishDomainCommand(validateSink);
        }

        /// <summary>
        /// 2.1 从银行账户聚合根，收到转账交易已验证的领域通知，通知转账交易聚合根确认。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="notification">转账交易已验证的领域通知</param>
        public void OnDomainNotification(IDomainNotifyingContext context, TransferTransactionValidatedDomainNotification notification)
        {
            var command = new ConfirmTransferTransactionValidatedDomainCommand(notification.TransactionId, notification.AccountType);

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 2.2 从银行账户聚合根，收到转账交易信息验证失败的通知，通知转账交易聚合根，取消交易
        /// </summary>
        /// <param name="context">领域通知执行上下文</param>
        /// <param name="notification">转账交易信息验证失败的通知</param>
        public void OnDomainNotification(IDomainNotifyingContext context, TransferTransactionValidateFailedDomainNotification notification)
        {
            context.PublishDomainCommand(new CancelTransferTransactionDomainCommand(notification.TransactionId, notification.Message));
        }

        /// <summary>
        /// 2.3 从转账交易聚合根，收到转账交易已取消的领域事件，通知银行账户聚合根释放在途交易。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">交易信息已准备就绪的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, TransferTransactionCancelledDomainEvent @event)
        {
            var sourceAccount = @event.SourceAccount;
            var sinkAccount = @event.SinkAccount;

            if (!sourceAccount.Validated)
            {
                context.PublishDomainCommand(new FreeTransferAccountTransactionDomainCommand(sourceAccount.Id, @event.AggregateRootId));
            }

            if (!sinkAccount.Validated)
            {
                context.PublishDomainCommand(new FreeTransferAccountTransactionDomainCommand(sinkAccount.Id, @event.AggregateRootId));
            }
        }

        /// <summary>
        /// 3. 从转账交易聚合根，收到交易信息已准备就绪的领域事件，向银行账户聚合根，向银行账户提交转账交易。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">交易信息已准备就绪的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, TransferTransactionReadiedDomainEvent @event)
        {
            // 1. 从源账户取款
            var withdraw = new SubmitTransferTransactionDomainCommand(@event.AggregateRootId, @event.SourceAccountId);

            context.PublishDomainCommand(withdraw);

            // 2. 存到目标账户
            var deposit = new SubmitTransferTransactionDomainCommand(@event.AggregateRootId, @event.SinkAccountId);

            context.PublishDomainCommand(deposit);
        }

        /// <summary>
        /// 4 从银行账户聚合根，收到转账交易已提交的领域事件，通知转账交易聚合根确认。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">账交易已提交的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, TransferTransactionSubmittedDomainEvent @event)
        {
            var command = new ConfirmTransferTransactionSubmittedDomainCommand(@event.TransactionId, @event.AccountType, @event.AccountBalance, @event.AccountInAmountInFlight, @event.AccountOutAmountInFlight);

            context.PublishDomainCommand(command);
        }
    }
}