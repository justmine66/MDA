using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageQueueService
    {
        MessageBusClientNames Name { get; }

        void Start();

        void Enqueue(IMessage message);

        void Stop();
    }

    public interface IAsyncMessageQueueService
    {
        MessageBusClientNames Name { get; }

        Task StartAsync(CancellationToken token = default);

        Task EnqueueAsync(IMessage message, CancellationToken token = default);

        Task StopAsync(CancellationToken token = default);
    }
}
