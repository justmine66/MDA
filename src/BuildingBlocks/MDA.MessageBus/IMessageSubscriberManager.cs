using System.Collections.Generic;

namespace MDA.MessageBus
{
    /// <summary>
    /// 消息订阅者管理器
    /// </summary>
    public interface IMessageSubscriberManager : IMessageSubscriber
    {
        bool IsEmpty { get; }

        bool HasSubscriber<TMessage>()
            where TMessage : Message;
        bool HasSubscriber(string topic);

        IEnumerable<MessageSubscriberDescriptor> GetSubscribers<TMessage>()
            where TMessage : Message;
        IEnumerable<MessageSubscriberDescriptor> GetSubscribers(string topic);

        void Clear();

        string GetMessageName<TMessage>()
            where TMessage : Message;
    }
}
