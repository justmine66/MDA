using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class StartupHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IMessageQueueService> _messageQueueServices;
        private readonly IEnumerable<IAsyncMessageQueueService> _asyncMessageQueueServices;

        public StartupHostedService(
            IEnumerable<IMessageQueueService> messageQueueServices,
            IEnumerable<IAsyncMessageQueueService> asyncMessageQueueServices, 
            ILogger<StartupHostedService> logger)
        {
            _messageQueueServices = messageQueueServices;
            _asyncMessageQueueServices = asyncMessageQueueServices;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_messageQueueServices != null)
            {
                foreach (var messageQueueService in _messageQueueServices)
                {
                    messageQueueService.Start();

                    _logger.LogInformation($"The {messageQueueService.Name} message queue service started.");
                }
            }

            if (_asyncMessageQueueServices != null)
            {
                foreach (var messageQueueService in _asyncMessageQueueServices)
                {
                    await messageQueueService.StartAsync(cancellationToken).ConfigureAwait(false);

                    _logger.LogInformation($"The {messageQueueService.Name} async message queue service started.");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_messageQueueServices != null)
            {
                foreach (var messageQueueService in _messageQueueServices)
                {
                    messageQueueService.Stop();

                    _logger.LogInformation($"The {messageQueueService.Name} message queue service stopped.");
                }
            }

            if (_asyncMessageQueueServices != null)
            {
                foreach (var messageQueueService in _asyncMessageQueueServices)
                {
                    await messageQueueService.StopAsync(cancellationToken).ConfigureAwait(false);

                    _logger.LogInformation($"The {messageQueueService.Name} async message queue service stopped.");
                }
            }
        }
    }
}
