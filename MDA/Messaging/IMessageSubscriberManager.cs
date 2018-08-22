using System.Collections.Generic;

namespace MDA.Messaging
{
    /// <summary>
    /// 消息订阅者管理器
    /// </summary>
    public interface IMessageSubscriberManager : IMessageSubscriber
    {
        bool IsEmpty { get; }

        bool HasSubscriberForMessage<TMessage>()
            where TMessage : IMessage;
        bool HasSubscriberForMessage(string messageName);

        IEnumerable<MessageSubscriberInfo> GetHandlersForMessage<TMessage>()
            where TMessage : IMessage;
        IEnumerable<MessageSubscriberInfo> GetHandlersForMessage(string messageName);

        void Clear();

        string GetMessageName<TMessage>()
            where TMessage : IMessage;
    }
}
