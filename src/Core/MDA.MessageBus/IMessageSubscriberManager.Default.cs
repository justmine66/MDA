using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessageSubscriberManager : IMessageSubscriberManager
    {
        private readonly ConcurrentDictionary<Type, List<MessageSubscriber>> _messageHandlerMap;

        public MessageSubscriberManager()
            => _messageHandlerMap = new ConcurrentDictionary<Type, List<MessageSubscriber>>();

        public void Subscribe(Type messageType, Type messageHandlerType)
        {
            var subscriber = new MessageSubscriber(messageType, messageHandlerType);

            _messageHandlerMap.AddOrUpdate(messageType,
                key => new List<MessageSubscriber>(),
                (key, oldValue) =>
                {
                    oldValue.Add(subscriber);

                    return oldValue;
                });
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriber(messageType, typeof(TMessageHandler));

            _messageHandlerMap.AddOrUpdate(messageType,
                key => new List<MessageSubscriber>() { subscriber },
                (key, oldValue) =>
                {
                    oldValue.Add(subscriber);

                    return oldValue;
                });
        }

        public async Task SubscribeAsync<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IAsyncMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriber(messageType, typeof(TMessageHandler));

            _messageHandlerMap.AddOrUpdate(messageType,
                key => new List<MessageSubscriber>() { subscriber },
                (key, oldValue) =>
                {
                    oldValue.Add(subscriber);

                    return oldValue;
                });

            await Task.CompletedTask;
        }

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriber(messageType, typeof(TMessageHandler));

            if (_messageHandlerMap.TryGetValue(messageType, out var subscribers))
            {
                subscribers.Remove(subscriber);
            }
        }

        public IEnumerable<MessageSubscriber> GetSubscribers(Type messageType)
            => _messageHandlerMap.TryGetValue(messageType, out var handlers) ? handlers : Enumerable.Empty<MessageSubscriber>();
    }
}
