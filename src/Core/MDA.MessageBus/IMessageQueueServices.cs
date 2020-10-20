using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageQueueService
    {
        Task StartAsync(CancellationToken token = default);

        void Enqueue(IMessage message);
        Task EnqueueAsync(IMessage message, CancellationToken token = default);

        Task StopAsync(CancellationToken token = default);
    }
}
