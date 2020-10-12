using MDA.Domain.Commands;
using MDA.MessageBus;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.UI.CTL
{
    public class StartupHostedService : IHostedService
    {
        private readonly IMessageQueueService _messageQueueService;
        private readonly IMessageSubscriberManager _subscriber;

        public StartupHostedService(
            IMessageQueueService messageQueueService,
            IMessageSubscriberManager subscriber)
        {
            _messageQueueService = messageQueueService;
            _subscriber = subscriber;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.SubscribeDomainCommands();

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