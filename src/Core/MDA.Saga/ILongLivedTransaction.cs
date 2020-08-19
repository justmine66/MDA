namespace MDA.Saga
{
    /// <summary>
    /// 表示一个需要长时间运行的事物
    /// </summary>
    public interface ILongLivedTransaction
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        bool IsCompleted { get;  }

        /// <summary>
        /// 是否失败
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// 是否超时
        /// </summary>
        bool IsTimeout { get; }

        /// <summary>
        /// 允许处理时长
        /// </summary>
        long AllowableDuration { get; }

        /// <summary>
        /// 当前处理时长
        /// </summary>
        long CurrentDuration { get; }
    }
}
