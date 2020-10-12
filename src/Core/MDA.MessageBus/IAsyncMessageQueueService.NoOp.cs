using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class NoOpAsyncMessageQueueService : IAsyncMessageQueueService
    {
        public async Task StartAsync(CancellationToken token = default)
        {
            await Task.CompletedTask;
        }

        public async Task EnqueueAsync(IMessage message, CancellationToken token = default)
        {
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken token = default)
        {
            await Task.CompletedTask;
        }
    }
}
