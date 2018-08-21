namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个消息订阅者。
    /// </summary>
    public interface IMessageSubscriber
    {
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler;

        void Subscribe<TMessageHandler>(string messageName)
            where TMessageHandler : IDynamicMessageHandler;

        void UnsubscribeDynamic<TMessageHandler>(string eventName)
            where TMessageHandler : IDynamicMessageHandler;

        void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler;
    }
}
