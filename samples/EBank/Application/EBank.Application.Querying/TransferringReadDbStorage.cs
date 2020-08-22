using EBank.Domain.Events.Transferring;
using MDA.Domain.Events;
using Microsoft.Extensions.Logging;

namespace EBank.Application.Querying
{
    public class TransferringReadDbStorage :
        IDomainEventHandler<TransferTransactionStartedDomainEvent>
    {
        private readonly ILogger _logger;

        public TransferringReadDbStorage(ILogger<TransferringReadDbStorage> logger)
        {
            _logger = logger;
        }

        public void Handle(TransferTransactionStartedDomainEvent @event)
        {
            _logger.LogInformation($"发起一笔转账交易: {@event.AggregateRootId}，金额：{@event.Amount}，已记录到读库.");
        }
    }
}