using System.Collections.Generic;
using System.Threading.Tasks;
namespace MDA.MessageBus
{
    /// <summary>
    /// 表示一个消息发布者。
    /// </summary>
    public interface IMessagePublisher 
    {
        /// <summary>
        /// 发布单条消息。
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="message">消息。</param>
        /// <returns></returns>
        Task PublishAsync(string topic, dynamic message);

        /// <summary>
        /// 发布单条消息。
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="message">消息</param>
        /// <returns></returns>
        Task PublishAsync<TMessage>(TMessage message)
            where TMessage : Message;

        /// <summary>
        /// 发布一组消息。
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messages">消息。</param>
        /// <returns></returns>
        Task PublishAllAsync(string topic, IEnumerable<dynamic> messages);

        /// <summary>
        /// 发布一组消息。
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="messages">一组消息</param>
        /// <returns></returns>
        Task PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : Message;
    }
}
