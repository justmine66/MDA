﻿using System;
using System.Collections.Generic;

namespace MDA.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IMessagePublisher _publisher;
        private readonly IMessageSubscriber _subscriber;

        public MessageBus(IMessagePublisher publisher, IMessageSubscriber subscriber)
        {
            _publisher = publisher;
            _subscriber = subscriber;
        }

        public void Publish(IMessage message) => _publisher.Publish(message);

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
            => _subscriber.Subscribe<TMessage, TMessageHandler>();

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
            => _subscriber.Unsubscribe<TMessage, TMessageHandler>();

        public IEnumerable<MessageSubscriberInfo> GetSubscribers(Type messageType)
            => _subscriber.GetSubscribers(messageType);
    }
}
