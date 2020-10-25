using MDA.MessageBus;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.BusinessServer.Bootstrap
{
    public class StartupHostedService : IHostedService
    {
        private readonly IMessageQueueService _messageQueueService;

        public StartupHostedService(IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageQueueService.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _messageQueueService.StopAsync(cancellationToken);
        }
    }
}
