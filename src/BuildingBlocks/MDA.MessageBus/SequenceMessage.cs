namespace MDA.MessageBus
{
    /// <summary>
    /// 表示一个有序消息
    /// </summary>
    public class SequenceMessage : Message
    {
        /// <summary>
        /// 序号
        /// </summary>
        public long Sequence { get; set; }
    }
}
