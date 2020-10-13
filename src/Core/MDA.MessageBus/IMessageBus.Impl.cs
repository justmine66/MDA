using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IMessagePublisher _publisher;
        private readonly IMessageSubscriberManager _subscriber;

        public MessageBus(IMessagePublisher publisher, IMessageSubscriberManager subscriber)
        {
            _publisher = publisher;
            _subscriber = subscriber;
        }

        public void Publish(IMessage message) 
            => _publisher.Publish(message);

        public async Task PublishAsync(IMessage message, CancellationToken token) 
            => await _publisher.PublishAsync(message, token);

        public void Subscribe(Type messageType, Type messageHandlerType)
            => _subscriber.Subscribe(messageType, messageHandlerType);

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
            => _subscriber.Subscribe<TMessage, TMessageHandler>();

        public async Task SubscribeAsync<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IAsyncMessageHandler<TMessage>
            => await _subscriber.SubscribeAsync<TMessage, TMessageHandler>();

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
            => _subscriber.Unsubscribe<TMessage, TMessageHandler>();

        public IEnumerable<MessageSubscriber> GetSubscribers(Type messageType)
            => _subscriber.GetSubscribers(messageType);

        public IEnumerable<MessageSubscriber> GetAllSubscribers()
            => _subscriber.GetAllSubscribers();
    }
}
