using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageQueueService _queueService;

        public MessagePublisher(IMessageQueueService queueService) => _queueService = queueService;

        public void Publish(IMessage message) => DoPublish(message);

        public async Task PublishAsync(IMessage message, CancellationToken token)
        {
            DoPublish(message);

            await Task.CompletedTask;
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
