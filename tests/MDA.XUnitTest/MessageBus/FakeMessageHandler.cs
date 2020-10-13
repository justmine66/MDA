using MDA.MessageBus;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.XUnitTest.MessageBus
{
    public class FakeMessageHandler : IMessageHandler<FakeMessage>
    {
        private readonly ILogger<FakeMessageHandler> _logger;

        public FakeMessageHandler(ILogger<FakeMessageHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(FakeMessage message)
        {
            _logger.LogInformation($"The message: {nameof(FakeMessage)}[Payload: {message.Payload}] handled.");
        }
    }

    public class AsyncFakeMessageHandler : IAsyncMessageHandler<FakeMessage>
    {
        private readonly ILogger<AsyncFakeMessageHandler> _logger;

        public AsyncFakeMessageHandler(ILogger<AsyncFakeMessageHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(FakeMessage message, CancellationToken token = default)
        {
            _logger.LogInformation($"The message: {nameof(FakeMessage)}[Payload: {message.Payload}] handled.");

            await Task.CompletedTask;
        }
    }

    public class MultiMessageHandler : IMessageHandler<FakeMessage>, IAsyncMessageHandler<FakeMessage>
    {
        private readonly ILogger<MultiMessageHandler> _logger;

        public MultiMessageHandler(ILogger<MultiMessageHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(FakeMessage message)
        {
            _logger.LogInformation($"The message: {nameof(FakeMessage)}[Payload: {message.Payload}] handled.");
        }

        public async Task HandleAsync(FakeMessage message, CancellationToken token = default)
        {
            _logger.LogInformation($"The message: {nameof(FakeMessage)}[Payload: {message.Payload}] handled.");

            await Task.CompletedTask;
        }
    }
}
