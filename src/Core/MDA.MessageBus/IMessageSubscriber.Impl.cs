﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly ConcurrentDictionary<Type, List<MessageSubscriberInfo>> _state;

        public MessageSubscriber()
        {
            _state = new ConcurrentDictionary<Type, List<MessageSubscriberInfo>>();
        }

        public void Subscribe(Type messageType, Type messageHandlerType)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriberInfo(messageType, typeof(TMessageHandler));

            _state.AddOrUpdate(messageType, key => new List<MessageSubscriberInfo>() { subscriber }, (key, oldValue) => oldValue);
        }

        public async Task SubscribeAsync<TMessage, TMessageHandler>() 
            where TMessage : IMessage 
            where TMessageHandler : IAsyncMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriberInfo(messageType, typeof(TMessageHandler));

            _state.AddOrUpdate(messageType, key => new List<MessageSubscriberInfo>() { subscriber }, (key, oldValue) => oldValue);

            await Task.CompletedTask;
        }

        public void Unsubscribe<TMessage, TMessageHandler>() 
            where TMessage : IMessage 
            where TMessageHandler : IMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriberInfo(messageType, typeof(TMessageHandler));

            if (_state.TryGetValue(messageType, out var subscribers))
            {
                subscribers.Remove(subscriber);
            }
        }

        public IEnumerable<MessageSubscriberInfo> GetSubscribers(Type messageType)
            => _state.TryGetValue(messageType, out var handlers) ? handlers : Enumerable.Empty<MessageSubscriberInfo>();
    }
}