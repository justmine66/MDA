using MDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Messaging.Impl
{
    /// <summary>
    /// 表示一个内存消息订阅器。
    /// </summary>
    public class InMemoryMessageSubscriberManager : IMessageSubscriberManager
    {
        private readonly Dictionary<string, List<MessageSubscriberInfo>> _subscribers;

        public InMemoryMessageSubscriberManager()
        {
            _subscribers = new Dictionary<string, List<MessageSubscriberInfo>>();
        }

        public bool IsEmpty => _subscribers.Any();

        public void Clear()
        {
            _subscribers.Clear();
        }

        public IEnumerable<MessageSubscriberInfo> GetSubscribers<TMessage>() where TMessage : IMessage
        {
            return _subscribers[GetMessageName<TMessage>()];
        }

        public IEnumerable<MessageSubscriberInfo> GetSubscribers(string messageName)
        {
            Assert.NotNullOrEmpty(messageName, nameof(messageName));

            return _subscribers[messageName];
        }

        public string GetMessageName<TMessage>()
            where TMessage : IMessage
        {
            return typeof(TMessage).Name;
        }

        public bool HasSubscriber<TMessage>()
            where TMessage : IMessage
        {
            return _subscribers[GetMessageName<TMessage>()].Any();
        }

        public bool HasSubscriber(string messageName)
        {
            Assert.NotNullOrEmpty(messageName, nameof(messageName));

            return _subscribers[messageName].Any();
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            DoAddSubscriber(typeof(TMessageHandler), GetMessageName<TMessage>(), isDynamic: false);
        }

        public void SubscribeDynamic<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            DoAddSubscriber(typeof(TMessageHandler), messageName, isDynamic: true);
        }

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            DoRemoveSubcriber(GetMessageName<TMessage>(), MessageSubscriberInfo.Typed(typeof(TMessageHandler)));
        }

        public void UnsubscribeDynamic<TMessageHandler>(string messageName)
            where TMessageHandler : IDynamicMessageHandler
        {
            DoRemoveSubcriber(messageName, MessageSubscriberInfo.Dynamic(typeof(TMessageHandler)));
        }

        private void DoAddSubscriber(
            Type handlerType,
            string messageName,
            bool isDynamic)
        {
            Assert.NotNull(handlerType, nameof(handlerType));
            Assert.NotNullOrEmpty(messageName, nameof(messageName));

            if (!_subscribers.ContainsKey(messageName))
            {
                _subscribers.Add(messageName, new List<MessageSubscriberInfo>());
            }
            else
            {
                if (_subscribers[messageName].Any(s => s.HandlerType == handlerType))
                {
                    throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{messageName}'", nameof(handlerType));
                }

                if (isDynamic)
                {
                    _subscribers[messageName].Add(MessageSubscriberInfo.Dynamic(handlerType));
                }
                else
                {
                    _subscribers[messageName].Add(MessageSubscriberInfo.Typed(handlerType));
                }
            }
        }

        private void DoRemoveSubcriber(string messageName, MessageSubscriberInfo subsToRemove)
        {
            if (string.IsNullOrEmpty(messageName) ||
                subsToRemove == null) return;

            if (_subscribers[messageName].Any())
            {
                _subscribers[messageName].Remove(subsToRemove);

                if (!_subscribers[messageName].Any())
                {
                    _subscribers.Remove(messageName);
                }
            }
        }
    }
}
