using System;
using System.Collections.Generic;

namespace MDA.MessageBus
{
    public interface IMessageSubscriber
    {
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        IEnumerable<MessageSubscriberInfo> GetMessageSubscribers(Type messageType);
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
