using EBank.Domain.Commands.Accounts;
using EBank.Domain.Commands.Withdrawing;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Withdrawing;
using EBank.Domain.Notifications.Accounts;
using MDA.Domain.Events;
using MDA.Domain.Notifications;

namespace EBank.BusinessServer.ProcessorManagers
{
    /// <summary>
    /// 取款交易流程管理器
    /// 协调整个取款交易事务
    /// </summary>
    public class WithdrawTransactionProcessorManager :
        // 1. 从取款交易聚合根，收到取款交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        IDomainEventHandler<WithdrawTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到取款交易信息验证已通过的通知，通知取款交易聚合根确认。
        IDomainNotificationHandler<WithdrawTransactionValidatedDomainNotification>,
        // 2.2 从银行账户聚合根，收到取款交易信息验证已失败的通知，向取款账交易聚合根发起取消交易。
        IDomainNotificationHandler<WithdrawTransactionValidateFailedDomainNotification>,
        // 3.1 从取款交易聚合根，收到取款交易已准备就绪的领域事件，向银行账户聚合更提交交易。
        IDomainEventHandler<WithdrawTransactionReadiedDomainEvent>,
        // 4. 从银行账户聚合根，收到取款账户交易已提交的领域事件，通知取款交易聚合根确认。
        IDomainEventHandler<WithdrawTransactionSubmittedDomainEvent>
    {
        /// <summary>
        /// 1. 从取款交易聚合根，收到取款交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">交易已发起的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, WithdrawTransactionStartedDomainEvent @event)
        {
            var command = new ValidateWithdrawTransactionDomainCommand(@event.AggregateRootId, @event.AccountId, @event.AccountName, @event.Bank, @event.Money);

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 2.1 从银行账户聚合根，收到取款交易信息验证已通过的领域事件，通知取款交易聚合根确认。
        /// </summary>
        /// <param name="context">通知处理上下文</param>
        /// <param name="notification">交易信息验证已通过的领域通知</param>
        public void OnDomainNotification(IDomainNotifyingContext context, WithdrawTransactionValidatedDomainNotification notification)
        {
            var command = new ConfirmWithdrawTransactionValidatedDomainCommand()
            {
                AggregateRootId = notification.TransactionId
            };

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 2.2 从银行账户聚合根，收到取款交易信息验证已失败的通知，向取款账交易聚合根发起取消交易。
        /// </summary>
        /// <param name="context">领域通知处理上下文</param>
        /// <param name="notification">取款交易信息验证已失败的通知</param>
        public void OnDomainNotification(IDomainNotifyingContext context, WithdrawTransactionValidateFailedDomainNotification notification)
        {
            var command = new CancelWithdrawTransactionDomainCommand()
            {
                AggregateRootId = notification.TransactionId,
                Message = notification.Message
            };

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 3.1 从取款交易聚合根，收到取款交易已准备就绪的领域事件，向银行账户聚合更提交交易。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event"></param>
        public void OnDomainEvent(IDomainEventingContext context, WithdrawTransactionReadiedDomainEvent @event)
        {
            var command = new SubmitWithdrawTransactionDomainCommand(@event.AggregateRootId, @event.AccountId);

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 4. 从银行账户聚合根，收到取款账户交易已提交的领域事件，通知取款交易聚合根确认。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event"></param>
        public void OnDomainEvent(IDomainEventingContext context, WithdrawTransactionSubmittedDomainEvent @event)
        {
            var command = new ConfirmWithdrawTransactionSubmittedDomainCommand(@event.TransactionId, @event.AccountBalance, @event.AccountInAmountInFlight, @event.AccountOutAmountInFlight);

            context.PublishDomainCommand(command);
        }
    }
}