using System.Threading;
using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Transferring;
using EBank.Application.Notifications.Transferring;
using EBank.Domain.Events.Transferring;
using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Transferring;
using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Events;
using System.Threading.Tasks;
using EBank.Domain.Events.Accounts;

namespace EBank.Application.BusinessServer.ProcessorManagers
{
    /// <summary>
    /// 转账交易处理器
    /// 协调整个转账交易流程
    /// </summary>
    public class TransferringTransactionProcessorManager :
        // 1. 从转账交易聚合根，收到交易已发起的领域事件，向银行账户聚合根发起交易信息验证。
        IAsyncDomainEventHandler<TransferTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到转账交易信息验证通过的通知，通知转账交易聚合根确认。
        IApplicationNotificationHandler<TransferTransactionAccountValidatePassedApplicationNotification>,
        // 2.2 从银行账户聚合根，收到转账交易信息验证失败的通知，通知转账交易聚合根，取消交易。
        IApplicationNotificationHandler<TransferTransactionAccountValidateFailedApplicationNotification>,
        // 3.1 从转账交易聚合根，收到交易信息已验证完成的领域事件，向银行账户聚合根，发起源账户扣款账户交易。
        IDomainEventHandler<TransferTransactionValidateCompletedDomainEvent>,
        // 3.2 从银行账户聚合根，收到源账户扣款完成的领域事件，通知转账交易聚合根确认。
        IDomainEventHandler<WithdrawAccountTransactionCompletedDomainEvent>,
        // 4.1 从转账交易聚合根，收到已确认源账户完成扣款账户交易的领域事件，向银行账户聚合根，发起目标账户存款账户交易。
        IDomainEventHandler<WithdrawAccountTransactionCompleteConfirmedDomainEvent>,
        // 4.2 从银行账户聚合根，收到目标账户存款完成的领域事件，通知转账交易聚合根确认。
        IDomainEventHandler<DepositAccountTransactionCompletedDomainEvent>,
        // 5. 从转账交易聚合根，收到转账交易已完成的领域事件。
        IDomainEventHandler<TransferTransactionCompletedDomainEvent>
    {
        private readonly IApplicationCommandService _commandService;

        public TransferringTransactionProcessorManager(IApplicationCommandService commandService)
            => _commandService = commandService;

        public async Task HandleAsync(TransferTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            // 1. 验证源账户
            var validateSource = new ValidateTransferTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                Account = @event.SourceAccount,
                AccountType = TransferTransactionAccountType.Source
            };

            await _commandService.PublishAsync(validateSource, token);

            // 2. 验证目标账户
            var validateSink = new ValidateTransferTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                Account = @event.SinkAccount,
                AccountType = TransferTransactionAccountType.Sink
            };

            await _commandService.PublishAsync(validateSink, token);
        }

        public void Handle(TransferTransactionAccountValidatePassedApplicationNotification notification)
        {
            var command = new ConfirmTransferTransactionValidatePassedApplicationCommand()
            {
                TransactionId = notification.TransactionId,
                AccountType = notification.AccountType
            };

            _commandService.Publish(command);
        }

        public void Handle(TransferTransactionAccountValidateFailedApplicationNotification notification)
        {
            var command = new CancelTransferTransactionApplicationCommand()
            {
                TransactionId = notification.TransactionId
            };

            _commandService.Publish(command);
        }

        public void Handle(TransferTransactionValidateCompletedDomainEvent @event)
        {
            var sourceAccount = @event.SourceAccount;
            var appCommand = new StartWithdrawAccountTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                AccountId = sourceAccount.Id,
                AccountName = sourceAccount.Name,
                Bank = sourceAccount.Bank,
                TransactionStage = AccountTransactionStage.Commitment
            };

            _commandService.Publish(appCommand);
        }

        public void Handle(WithdrawAccountTransactionCompletedDomainEvent @event)
        {
            var appCommand = new ConfirmWithdrawAccountTransactionCompletedApplicationCommand()
            {
                TransactionId = @event.TransactionId,
                AccountId = @event.AggregateRootId
            };

            _commandService.Publish(appCommand);
        }

        public void Handle(WithdrawAccountTransactionCompleteConfirmedDomainEvent @event)
        {
            var sourceAccount = @event.Sink;
            var appCommand = new StartDepositAccountTransactionApplicationCommand()
            {
                TransactionId = @event.AggregateRootId,
                AccountId = sourceAccount.Id,
                AccountName = sourceAccount.Name,
                Bank = sourceAccount.Bank,
                TransactionStage = AccountTransactionStage.Commitment
            };

            _commandService.Publish(appCommand);
        }

        public void Handle(DepositAccountTransactionCompletedDomainEvent @event)
        {
            var appCommand = new ConfirmDepositAccountTransactionCompletedApplicationCommand()
            {
                TransactionId = @event.TransactionId,
                AccountId = @event.AggregateRootId
            };

            _commandService.Publish(appCommand);
        }

        public void Handle(TransferTransactionCompletedDomainEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
