using System.Threading.Tasks;

namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个动态消息处理器
    /// </summary>
    public interface IDynamicMessageHandler
    {
        /// <summary>
        /// 处理消息。
        /// </summary>
        /// <param name="message">动态消息</param>
        /// <returns></returns>
        Task HandleAsync(dynamic message);
    }
}
