using MDA.Domain.Events;
using Microsoft.Extensions.Logging;

namespace MDA.Tests.BusinessProcessor
{
    public class ReadDbStorage : IDomainEventHandler<LongDomainEvent>
    {
        private readonly ILogger _logger;

        public ReadDbStorage(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(LongDomainEvent @event)
        {
            _logger.LogInformation($"{@event.AggregateRootTypeFullName}.{@event.AggregateRootId}/{@event.GetTypeFullName()}.{@event.Id}已存储到读库, 数据: {@event.Value}。");
        }
    }
}
