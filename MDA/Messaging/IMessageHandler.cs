using System.Threading.Tasks;

namespace MDA.Messaging
{
    public interface IMessageHandler { }
    /// <summary>
    /// 表示一个消息处理器。
    /// </summary>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        /// <summary>
        /// 处理消息。
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        Task HandleAsync(TMessage message);
    }
}
