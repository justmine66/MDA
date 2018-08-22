using System.Threading.Tasks;

namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个消息处理器。
    /// </summary>
    /// <typeparam name="TIMessage">消息</typeparam>
    public interface IMessageHandler
    {
        /// <summary>
        /// 处理消息。
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        Task HandleAsync(IMessage message);
    }
}
