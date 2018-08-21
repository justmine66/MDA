using MDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Messaging.Impl
{
    /// <summary>
    /// 表示一个内存消息订阅器。
    /// </summary>
    public class InMemoryMessageSubscriber : IMessageSubscriber
    {
        private readonly Dictionary<string, List<SubscriberInfo>> subscribers;

        public InMemoryMessageSubscriber()
        {
            this.subscribers = new Dictionary<string, List<SubscriberInfo>>();
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            DoAddSubscriber(typeof(TMessageHandler), typeof(TMessage).Name, isDynamic: false);
        }

        public void Subscribe<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            DoAddSubscriber(typeof(TMessageHandler), messageName, isDynamic: true);
        }

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            DoRemoveSubcriber(typeof(TMessage).Name, SubscriberInfo.Typed(typeof(TMessageHandler)));
        }

        public void UnsubscribeDynamic<TMessageHandler>(string messageName) 
            where TMessageHandler : IDynamicMessageHandler
        {
            DoRemoveSubcriber(messageName, SubscriberInfo.Dynamic(typeof(TMessageHandler)));
        }

        /// <summary>
        /// 订阅者信息。
        /// </summary>
        public class SubscriberInfo
        {
            /// <summary>
            /// 是否动态消息
            /// </summary>
            public bool IsDynamic { get; }
            /// <summary>
            /// 处理者类型
            /// </summary>
            public Type HandlerType { get; }

            /// <summary>
            /// 初始化一个 <see cref="SubscriberInfo"/> 实例。
            /// </summary>
            /// <param name="isDynamic">是否动态消息</param>
            /// <param name="handlerType">处理者类型</param>
            public SubscriberInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            /// <summary>
            /// 动态订阅者。
            /// </summary>
            /// <param name="handlerType">处理者类型</param>
            /// <returns></returns>
            public static SubscriberInfo Dynamic(Type handlerType)
            {
                return new SubscriberInfo(true, handlerType);
            }

            /// <summary>
            /// 类型化订阅者。
            /// </summary>
            /// <param name="handlerType">处理者类型</param>
            /// <returns></returns>
            public static SubscriberInfo Typed(Type handlerType)
            {
                return new SubscriberInfo(false, handlerType);
            }
        }

        private void DoAddSubscriber(
            Type handlerType,
            string messageName,
            bool isDynamic)
        {
            Assert.NotNull(handlerType, nameof(handlerType));
            Assert.NotNullOrEmpty(messageName, nameof(messageName));

            if (!this.subscribers.ContainsKey(messageName))
            {
                this.subscribers.Add(messageName, new List<SubscriberInfo>());
            }
            else
            {
                if (this.subscribers[messageName].Any(s => s.HandlerType == handlerType))
                {
                    throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{messageName}'", nameof(handlerType));
                }

                if (isDynamic)
                {
                    this.subscribers[messageName].Add(SubscriberInfo.Dynamic(handlerType));
                }
                else
                {
                    this.subscribers[messageName].Add(SubscriberInfo.Typed(handlerType));
                }
            }
        }

        private void DoRemoveSubcriber(string messageName, SubscriberInfo subsToRemove)
        {
            if (string.IsNullOrEmpty(messageName) ||
                subsToRemove != null) return;

            if (this.subscribers[messageName].Any())
            {
                this.subscribers[messageName].Remove(subsToRemove);

                if (!this.subscribers[messageName].Any())
                {
                    this.subscribers.Remove(messageName);
                }
            }
        }
    }
}
