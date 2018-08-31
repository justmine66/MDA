using System.Collections.Generic;

namespace MDA.Message.Abstractions
{
    /// <summary>
    /// 消息订阅者管理器
    /// </summary>
    public interface IMessageSubscriberManager : IMessageSubscriber
    {
        bool IsEmpty { get; }

        bool HasSubscriber<TMessage>()
            where TMessage : IMessage;
        bool HasSubscriber(string messageName);

        IEnumerable<MessageSubscriberDescriptor> GetSubscribers<TMessage>()
            where TMessage : IMessage;
        IEnumerable<MessageSubscriberDescriptor> GetSubscribers(string messageName);

        void Clear();

        string GetMessageName<TMessage>()
            where TMessage : IMessage;
    }
}
