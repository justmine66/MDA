using EBank.Domain.Events.Accounts;
using MDA.Domain.Events;
using Microsoft.Extensions.Logging;

namespace EBank.Application.QueryServer
{
    public class BankAccountReadDbStorage :
        IDomainEventHandler<AccountOpenedDomainEvent>,
        IDomainEventHandler<DepositAccountTransactionCompletedDomainEvent>,
        IDomainEventHandler<WithdrawAccountTransactionCompletedDomainEvent>
    {
        private readonly ILogger _logger;

        public BankAccountReadDbStorage(ILogger<BankAccountReadDbStorage> logger)
        {
            _logger = logger;
        }

        public void Handle(AccountOpenedDomainEvent @event)
        {
            _logger.LogInformation($"账户: {@event.AggregateRootId} 开户信息已同步到读库.");
        }

        public void Handle(DepositAccountTransactionCompletedDomainEvent @event)
        {
            _logger.LogInformation($"账户: {@event.AggregateRootId} 存款信息已同步到读库，金额：{@event.Amount}.");
        }

        public void Handle(WithdrawAccountTransactionCompletedDomainEvent @event)
        {
            _logger.LogInformation($"账户: {@event.AggregateRootId} 取款信息已同步到读库，金额：{@event.Amount}.");
        }
    }
}
