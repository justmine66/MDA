using MDA.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Messaging.Impl
{
    /// <summary>
    /// 表示一个内存消息总线。
    /// </summary>
    public class InMemoryMessageBus : IMessageBus
    {
        public InMemoryMessageBus() 
            : this("InMemoryMessageBus")
        {

        }

        public InMemoryMessageBus(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public Task<AsyncResult> PublishAllAsync<TMessage>(IEnumerable<TMessage> messages) where TMessage : IMessage
        {
            throw new NotImplementedException();
        }

        public Task<AsyncResult> PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDynamic<TMessageHandler>(string eventName) where TMessageHandler : IDynamicMessageHandler
        {
            throw new NotImplementedException();
        }
    }
}
