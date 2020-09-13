using System;

namespace MDA.MessageBus
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageQueueService _queueService;

        public MessagePublisher(IMessageQueueService queueService) => _queueService = queueService;

        public void Publish(IMessage message) => DoPublish(message);

        public void Publish<TPayload>(IMessage<TPayload> message) => DoPublish(message);

        public void Publish<TId, TPayload>(IMessage<TId, TPayload> message) => DoPublish(message);

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
