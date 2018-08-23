using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个消息发布者。
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// 发布单条消息。
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="message">消息</param>
        /// <returns></returns>
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;

        /// <summary>
        /// 发布一组消息。
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="messages">一组消息</param>
        /// <returns></returns>
        Task PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : IMessage;

        /// <summary>
        /// 发布单条消息。
        /// </summary>
        /// <param name="messageName">消息名称。</param>
        /// <param name="message">消息。</param>
        /// <returns></returns>
        Task PublishDynamicAsync(string messageName, dynamic message);

        /// <summary>
        /// 发布一组消息。
        /// </summary>
        /// <param name="messageName">消息名称。</param>
        /// <param name="messages">消息。</param>
        /// <returns></returns>
        Task PublishAllDynamicAsync(string messageName, IEnumerable<dynamic> messages);
    }
}
