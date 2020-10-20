using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageQueueService _queueService;

        public MessagePublisher(IMessageQueueService queueService)
            => _queueService = queueService;

        public void Publish(IMessage message) => _queueService.Enqueue(message);

        public async Task PublishAsync(IMessage message, CancellationToken token)
            => await _queueService.EnqueueAsync(message, token);
    }
}
