using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Depositing;
using EBank.Application.Notifications.Depositing;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Depositing;
using EBank.Domain.Models.Accounts;
using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Events;

namespace EBank.Application.BusinessServer.ProcessorManagers
{
    /// <summary>
    /// 存款交易处理器
    /// 协调整个存款交易流程
    /// </summary>
    public class DepositTransactionProcessorManager :
        // 1. 从存款交易聚合根，收到存款交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        IDomainEventHandler<DepositTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到存款交易信息验证已通过的通知，通知存款交易聚合根确认。
        IApplicationNotificationHandler<DepositTransactionValidatePassedApplicationNotification>,
        // 2.2 从银行账户聚合根，收到存款交易信息验证已失败的通知，向存款账交易聚合根发起取消交易。
        IApplicationNotificationHandler<DepositTransactionValidateFailedApplicationNotification>,
        // 3. 从存款交易聚合根，收到存款交易信息验证已完成的领域事件，向银行账户聚合更发起存款类型的账户交易。
        IDomainEventHandler<DepositTransactionValidateCompletedDomainEvent>,
        // 4. 从银行账户聚合根，收到存款账户交易已完成的领域事件，通知存款交易聚合根确认。
        IDomainEventHandler<DepositAccountTransactionCompletedDomainEvent>
    {
        private readonly IApplicationCommandService _commandService;

        public DepositTransactionProcessorManager(IApplicationCommandService commandService)
            => _commandService = commandService;

        public void Handle(DepositTransactionStartedDomainEvent @event)
        {
            var command = new ValidateDepositTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                AccountId = @event.AccountId,
                AccountName = @event.AccountName,
                Bank = @event.Bank
            };

            _commandService.Publish(command);
        }

        public void Handle(DepositTransactionValidatePassedApplicationNotification notification)
        {
            var command = new ConfirmDepositTransactionValidatePassedApplicationCommand()
            {
                TransactionId = notification.TransactionId
            };

            _commandService.Publish(command);
        }

        public void Handle(DepositTransactionValidateFailedApplicationNotification notification)
        {
            var command = new CancelDepositTransactionApplicationCommand()
            {
                TransactionId = notification.TransactionId
            };

            _commandService.Publish(command);
        }

        public void Handle(DepositTransactionValidateCompletedDomainEvent @event)
        {
            var command = new StartDepositAccountTransactionApplicationCommand()
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

        public void Handle(DepositAccountTransactionCompletedDomainEvent @event)
        {
            var command = new ConfirmDepositTransactionCompletedApplicationCommand()
            {
                TransactionId = @event.TransactionId
            };

            _commandService.Publish(command);
        }
    }
}
