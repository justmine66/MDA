namespace MDA.MessageBus
{
    /// <summary>
    /// 表示一个消息订阅者。
    /// </summary>
    public interface IMessageSubscriber 
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TMessageHandler">消息处理者类型</typeparam>
        /// <param name="topic">主题</param>
        void Subscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler;

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="TMessageHandler">消息处理者类型</typeparam>
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>;

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="TMessageHandler">消息处理者类型</typeparam>
        /// <param name="topic">主题</param>
        void UnSubscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler;

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="TMessageHandler">消息处理者类型</typeparam>
        void UnSubscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>;
    }
}
