using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Withdrawing;
using EBank.Application.Notifications.Withdrawing;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Withdrawing;
using EBank.Domain.Models.Accounts;
using EBank.Domain.Notifications;
using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Events;
using MDA.Domain.Notifications;

namespace EBank.Application.BusinessServer.Processors
{
    /// <summary>
    /// 取款交易处理器
    /// 协调整个取款交易流程
    /// </summary>
    public class WithdrawTransactionProcessor :
        // 1. 从取款交易聚合根，收到取款交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        IDomainEventHandler<WithdrawTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到取款交易信息验证已通过的通知，通知取款交易聚合根确认。
        IApplicationNotificationHandler<WithdrawTransactionValidatePassedApplicationNotification>,
        // 2.2 从银行账户聚合根，收到取款交易信息验证已失败的通知，向取款账交易聚合根发起取消交易。
        IApplicationNotificationHandler<WithdrawTransactionValidateFailedApplicationNotification>,
        // 3.1 从取款交易聚合根，收到取款交易信息验证已完成的领域事件，向银行账户聚合更发起取款类型的账户交易。
        IDomainEventHandler<WithdrawTransactionValidateCompletedDomainEvent>,
        // 3.2 从银行账户聚合根，收到取款账户交易余额不足的领域通知，向取款账交易聚合根发起取消交易。
        IDomainNotificationHandler<TransactionAccountInsufficientBalanceDomainNotification>,
        // 4. 从银行账户聚合根，收到取款账户交易已完成的领域事件，通知取款交易聚合根确认。
        IDomainEventHandler<WithdrawAccountTransactionCompletedDomainEvent>
    {
        private readonly IApplicationCommandService _commandService;

        public WithdrawTransactionProcessor(IApplicationCommandService commandService)
            => _commandService = commandService;

        public void Handle(WithdrawTransactionStartedDomainEvent @event)
        {
            var command = new ValidateWithdrawTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                AccountId = @event.AccountId,
                AccountName = @event.AccountName,
                Bank = @event.Bank
            };

            _commandService.Publish(command);
        }

        public void Handle(WithdrawTransactionValidatePassedApplicationNotification notification)
        {
            var command = new ConfirmWithdrawTransactionValidatePassedApplicationCommand()
            {
                TransactionId = notification.TransactionId
            };

            _commandService.Publish(command);
        }

        public void Handle(WithdrawTransactionValidateFailedApplicationNotification notification)
        {
            var command = new CancelWithdrawTransactionApplicationCommand()
            {
                TransactionId = notification.TransactionId
            };

            _commandService.Publish(command);
        }

        public void Handle(WithdrawTransactionValidateCompletedDomainEvent @event)
        {
            var command = new StartWithdrawAccountTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                AccountId = @event.AccountId,
                AccountName = @event.AccountName,
                Bank = @event.Bank,
                Amount = @event.Amount,
                TransactionStage = AccountTransactionStage.Commitment
            };

            _commandService.Publish(command);
        }

        public void Handle(TransactionAccountInsufficientBalanceDomainNotification notification)
        {
            var command = new CancelWithdrawTransactionApplicationCommand()
            {
                TransactionId = notification.TransactionId
            };

            _commandService.Publish(command);
        }

        public void Handle(WithdrawAccountTransactionCompletedDomainEvent @event)
        {
            var command = new ConfirmWithdrawTransactionCompletedApplicationCommand()
            {
                TransactionId = @event.TransactionId
            };

            _commandService.Publish(command);
        }
    }
}
