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
        private readonly Dictionary<string, List<MessageSubscriberDescriptor>> _subscribers;

        public InMemoryMessageSubscriberManager()
        {
            _subscribers = new Dictionary<string, List<MessageSubscriberDescriptor>>();
        }

        public bool IsEmpty => _subscribers.Any();

        public void Clear()
        {
            _subscribers.Clear();
        }

        public IEnumerable<MessageSubscriberDescriptor> GetSubscribers<TMessage>() where TMessage : IMessage
        {
            return _subscribers[GetMessageName<TMessage>()];
        }

        public IEnumerable<MessageSubscriberDescriptor> GetSubscribers(string messageName)
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
            DoRemoveSubcriber(GetMessageName<TMessage>(), MessageSubscriberDescriptor.Typed(typeof(TMessageHandler)));
        }

        public void UnsubscribeDynamic<TMessageHandler>(string messageName)
            where TMessageHandler : IDynamicMessageHandler
        {
            DoRemoveSubcriber(messageName, MessageSubscriberDescriptor.Dynamic(typeof(TMessageHandler)));
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
                _subscribers.Add(messageName, new List<MessageSubscriberDescriptor>());
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
                    _subscribers[messageName].Add(MessageSubscriberDescriptor.Dynamic(handlerType));
                }
                else
                {
                    _subscribers[messageName].Add(MessageSubscriberDescriptor.Typed(handlerType));
                }
            }
        }

        private void DoRemoveSubcriber(string messageName, MessageSubscriberDescriptor subsToRemove)
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
