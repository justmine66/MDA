using System;

namespace MDA.Messaging
{
    public class MessagingOptions
    {
        /// <summary>
        /// 是否观察慢消息。
        /// </summary>
        /// <remarks>
        /// 记录消息处理的时间，根据参数 <see cref="SlowMsgThreshold"/> 来衡量消息处理的快慢。
        /// </remarks>
        public bool WatchSlowMessage { get; set; }
        /// <summary>
        /// 慢消息阈值。
        /// </summary>
        public TimeSpan SlowMsgThreshold { get; set; }
    }
}
