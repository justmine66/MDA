using MDA.MessageBus;
using Microsoft.Extensions.Logging;

namespace MDA.XUnitTest.MessageBus
{
    public class FakeMessageWithPartitionKeyHandler : IMessageHandler<FakeMessageWithPartitionKey>
    {
        private readonly ILogger<FakeMessageWithPartitionKeyHandler> _logger;

        public FakeMessageWithPartitionKeyHandler(ILogger<FakeMessageWithPartitionKeyHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(FakeMessageWithPartitionKey message)
        {
            _logger.LogInformation($"The message: {nameof(FakeMessageWithPartitionKey)}[Payload: {message.Payload}] handled.");
        }
    }
}
