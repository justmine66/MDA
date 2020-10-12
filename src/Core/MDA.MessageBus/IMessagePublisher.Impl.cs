using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageQueueService _queueService;
        private readonly IAsyncMessageQueueService _asyncQueueService;

        public MessagePublisher(
            IMessageQueueService queueService,
            IAsyncMessageQueueService asyncQueueService)
        {
            _queueService = queueService;
            _asyncQueueService = asyncQueueService;
        }

        public void Publish(IMessage message) => DoPublish(message);

        public async Task PublishAsync(IMessage message, CancellationToken token)
        {
            await _asyncQueueService.EnqueueAsync(message, token);
        }

        private void DoPublish(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _queueService.Enqueue(message);
        }
    }
}
