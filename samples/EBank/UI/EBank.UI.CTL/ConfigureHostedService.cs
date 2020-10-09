using MDA.Domain.Commands;
using MDA.MessageBus;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.UI.CTL
{
    public class ConfigureHostedService : IHostedService
    {
        private readonly IMessageQueueService _messageQueueService;
        private readonly IMessageSubscriber _subscriber;

        public ConfigureHostedService(
            IMessageQueueService messageQueueService,
            IMessageSubscriber subscriber)
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