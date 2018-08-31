namespace MDA.Message.Abstractions
{
    /// <summary>
    /// 表示一个序列消息。
    /// </summary>
    public interface ISequenceMessage : IMessage
    {
        /// <summary>
        /// 顺序号
        /// </summary>
        int Sequence { get; set; }
    }
}
