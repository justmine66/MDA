using MDA.MessageBus;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.UI.CTL
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
            _messageQueueService.Start();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _messageQueueService.Stop();

            await Task.CompletedTask;
        }
    }
}