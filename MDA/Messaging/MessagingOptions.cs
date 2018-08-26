using System;

namespace MDA.Messaging
{
    /// <summary>
    /// 消息配置选项
    /// </summary>
    public class MessagingOptions
    {
        /// <summary>
        /// 是否监视慢的消息处理者。
        /// </summary>
        /// <remarks>
        /// 记录消息处理的时间，根据参数 <see cref="SlowMsgThreshold"/> 来衡量消息处理的快慢。
        /// </remarks>
        public bool MonitorSlowMessageHandler { get; set; }
        /// <summary>
        /// 慢消息处理者阈值。
        /// </summary>
        /// <remarks>
        /// 消息处理耗时，默认100毫秒。
        /// </remarks>
        public TimeSpan SlowMessageHandlerThreshold { get; set; } = TimeSpan.FromMilliseconds(100);
    }
}
