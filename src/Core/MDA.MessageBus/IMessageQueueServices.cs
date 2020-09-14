using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageQueueService
    {
        void Start();

        void Enqueue(IMessage message);

        void Enqueue<TPayload>(IMessage<TPayload> message);

        void Stop();
    }

    public interface IAsyncMessageQueueService
    {
        Task StartAsync(CancellationToken token = default);

        Task EnqueueAsync(IMessage message, CancellationToken token = default);

        Task EnqueueAsync<TPayload>(IMessage<TPayload> message, CancellationToken token = default);

        Task StopAsync(CancellationToken token = default);
    }
}
