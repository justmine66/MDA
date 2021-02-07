using EBank.Domain.Commands.Accounts;
using EBank.Domain.Commands.Depositing;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Depositing;
using EBank.Domain.Notifications;
using MDA.Domain.Events;
using MDA.Domain.Notifications;

namespace EBank.BusinessServer.ProcessorManagers
{
    /// <summary>
    /// 存款交易处理管理器
    /// 协调整个存款交易事务
    /// </summary>
    public class DepositTransactionProcessorManager :
        // 1. 从存款交易聚合根，收到存款交易已发起的领域事件，向银行账户聚合根发起交易验证。
        IDomainEventHandler<DepositTransactionStartedDomainEvent>,
        // 2.1 从银行账户聚合根，收到存款交易验证已通过的领域事件，向存款交易聚合根发起确认。
        IDomainEventHandler<DepositTransactionValidatedDomainEvent>,
        // 2.2 从银行账户聚合根，收到存款交易信息验证已失败的通知，向存款账交易聚合根发起取消交易。
        IDomainNotificationHandler<DepositTransactionValidateFailedDomainNotification>,
        // 3. 从存款交易聚合根，收到存款交易已准备就绪的领域事件，向银行账户聚合跟提交存款交易。
        IDomainEventHandler<DepositTransactionReadiedDomainEvent>,
        // 4. 从银行账户聚合根，收到存款账户交易已提交的领域事件，向存款交易聚合根发起确认。
        IDomainEventHandler<DepositTransactionSubmittedDomainEvent>
    {
        /// <summary>
        /// 1. 从存款交易聚合根，收到存款交易已发起的领域事件，向银行账户聚合根发起交易验证。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">存款交易已发起的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, DepositTransactionStartedDomainEvent @event)
        {
            var command = new ValidateDepositTransactionDomainCommand()
            {
                TransactionId = @event.AggregateRootId,
                AggregateRootId = @event.AccountId,
                AccountName = @event.AccountName,
                Bank = @event.Bank,
                Money = @event.Money
            };

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 2.1 从银行账户聚合根，收到存款交易验证已通过的领域事件，向存款交易聚合根发起确认。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">存款交易信息验证已通过的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, DepositTransactionValidatedDomainEvent @event)
        {
            var command = new ConfirmDepositTransactionValidatedDomainCommand()
            {
                AggregateRootId = @event.TransactionId
            };

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 2.2 从银行账户聚合根，收到存款交易信息验证已失败的通知，向存款账交易聚合根发起取消交易。
        /// </summary>
        /// <param name="context">领域通知执行上下文</param>
        /// <param name="notification">存款交易信息验证已失败的领域事件</param>
        public void OnDomainNotification(IDomainNotifyingContext context, DepositTransactionValidateFailedDomainNotification notification)
        {
            var command = new CancelDepositTransactionDomainCommand()
            {
                AggregateRootId = notification.TransactionId,
                Message = notification.Message
            };

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 3. 从存款交易聚合根，收到存款交易已准备就绪的领域事件，向银行账户聚合跟提交存款交易。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event">存款交易已准备就绪的领域事件</param>
        public void OnDomainEvent(IDomainEventingContext context, DepositTransactionReadiedDomainEvent @event)
        {
            var command = new SubmitDepositTransactionDomainCommand(@event.AggregateRootId, @event.AccountId);

            context.PublishDomainCommand(command);
        }

        /// <summary>
        /// 4. 从银行账户聚合根，收到存款账户交易已提交的领域事件，向存款交易聚合根发起确认。
        /// </summary>
        /// <param name="context">事件处理上下文</param>
        /// <param name="event"></param>
        public void OnDomainEvent(IDomainEventingContext context, DepositTransactionSubmittedDomainEvent @event)
        {
            var command = new ConfirmDepositTransactionSubmittedDomainCommand(@event.TransactionId, @event.AccountBalance, @event.AccountInAmountInFlight, @event.AccountOutAmountInFlight);

            context.PublishDomainCommand(command);
        }
    }
}