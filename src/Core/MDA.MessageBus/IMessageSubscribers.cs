using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageSubscriber
    {
        void Subscribe(Type messageType, Type messageHandlerType);

        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        Task SubscribeAsync<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IAsyncMessageHandler<TMessage>;

        void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        IEnumerable<MessageSubscriberInfo> GetSubscribers(Type messageType);
    }

    public class MessageSubscriberInfo
    {
        public MessageSubscriberInfo() { }
        public MessageSubscriberInfo(Type messageType, Type messageHandlerType)
        {
            MessageType = messageType;
            MessageHandlerType = messageHandlerType;
        }

        public Type MessageType { get; set; }

        public Type MessageHandlerType { get; set; }
    }
}
