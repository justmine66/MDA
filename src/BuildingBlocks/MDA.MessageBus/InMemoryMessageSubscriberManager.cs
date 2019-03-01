using MDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.MessageBus
{
    /// <summary>
    /// 表示一个内存消息订阅器。
    /// </summary>
    public class InMemoryMessageSubscriberManager : IMessageSubscriberManager
    {
        private Dictionary<string, IMessageSubscriberCollection> _subscribers;
        private IMessageSubscriberCollection _subscriberCollection;

        public InMemoryMessageSubscriberManager(IMessageSubscriberCollection subscriberCollection)
        {
            _subscriberCollection = subscriberCollection;
            _subscribers = new Dictionary<string, IMessageSubscriberCollection>();
        }

        public bool IsEmpty => !_subscribers.Any();

        public void Clear()
        {
            _subscribers.Clear();
        }

        public IEnumerable<MessageSubscriberDescriptor> GetSubscribers<TMessage>()
            where TMessage : Message
        {
            return _subscribers[GetMessageName<TMessage>()];
        }

        public IEnumerable<MessageSubscriberDescriptor> GetSubscribers(string topic)
        {
            Assert.NotNullOrEmpty(topic, nameof(topic));

            return _subscribers[topic];
        }

        public string GetMessageName<TMessage>()
            where TMessage : Message
        {
            return typeof(TMessage).Name;
        }

        public bool HasSubscriber<TMessage>()
            where TMessage : Message
        {
            return _subscribers[GetMessageName<TMessage>()].Any();
        }

        public bool HasSubscriber(string topic)
        {
            Assert.NotNullOrEmpty(topic, nameof(topic));

            return _subscribers[topic].Any();
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>
        {
            DoAddSubscriber(typeof(TMessageHandler), typeof(TMessage), GetMessageName<TMessage>(), isDynamic: false);
        }

        public void Subscribe<TMessageHandler>(string topic) 
            where TMessageHandler : IDynamicMessageHandler
        {
            DoAddSubscriber(typeof(TMessageHandler), null, topic, isDynamic: true);
        }

        public void UnSubscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>
        {
            DoRemoveSubscriber(GetMessageName<TMessage>(), MessageSubscriberDescriptor.Typed(typeof(TMessage), typeof(TMessageHandler)));
        }

        public void UnSubscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler
        {
            DoRemoveSubscriber(topic, MessageSubscriberDescriptor.Dynamic(topic, typeof(TMessageHandler)));
        }

        private void DoAddSubscriber(
            Type handlerType,
            Type messageType,
            string topic,
            bool isDynamic)
        {
            Assert.NotNull(handlerType, nameof(handlerType));
            Assert.NotNullOrEmpty(topic, nameof(topic));

            if (!_subscribers.ContainsKey(topic))
            {
                _subscribers.Add(topic, _subscriberCollection.New());
            }

            if (_subscribers[topic].Any(s => s.MessageHandlerType == handlerType))
            {
                throw new ArgumentException(
                $"Handler Type {handlerType.Name} already registered for '{topic}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _subscribers[topic].Add(MessageSubscriberDescriptor.Dynamic(topic, handlerType));
            }
            else
            {
                Assert.NotNull(messageType, nameof(messageType));

                _subscribers[topic].Add(MessageSubscriberDescriptor.Typed(messageType, handlerType));
            }
        }

        private void DoRemoveSubscriber(string topic, MessageSubscriberDescriptor subsToRemove)
        {
            if (string.IsNullOrEmpty(topic) ||
                subsToRemove == null) return;

            if (_subscribers[topic].Any())
            {
                _subscribers[topic].Remove(subsToRemove);

                if (!_subscribers[topic].Any())
                {
                    _subscribers.Remove(topic);
                }
            }
        }

        public void Dispose()
        {
            _subscribers = null;
            _subscriberCollection = null;
        }
    }
}
