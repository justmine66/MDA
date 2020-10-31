using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class DefaultMessagePublisher : IMessagePublisher
    {
        private readonly IMessageQueueService _queueService;
        private readonly IAsyncMessageQueueService _asyncQueueService;

        public DefaultMessagePublisher(
            IMessageQueueService queueService,
            IAsyncMessageQueueService asyncQueueService)
        {
            _queueService = queueService;
            _asyncQueueService = asyncQueueService;
        }

        public void Publish(IMessage message) => _queueService.Enqueue(message);

        public async Task PublishAsync(IMessage message, CancellationToken token)
            => await _asyncQueueService.EnqueueAsync(message, token);
    }
}
