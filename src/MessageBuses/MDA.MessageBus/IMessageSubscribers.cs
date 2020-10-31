using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageSubscriberManager
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

        IEnumerable<MessageSubscriber> GetSubscribers(Type messageType);

        IEnumerable<MessageSubscriber> GetAllSubscribers();
    }

    public class MessageSubscriber
    {
        public MessageSubscriber() { }
        public MessageSubscriber(Type messageType, Type messageHandlerType)
        {
            MessageType = messageType;
            MessageHandlerType = messageHandlerType;
            IsAsynchronousMessageHandler = messageHandlerType.Name.StartsWith("IAsyncMessageHandler");
        }

        public Type MessageType { get; set; }

        public Type MessageHandlerType { get; set; }

        public bool IsAsynchronousMessageHandler { get; set; }
    }
}
