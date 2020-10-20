namespace MDA.Domain.Events
{
    /// <summary>
    /// 领域事件声明周期
    /// </summary>
    public enum DomainEventLifetime
    {
        /// <summary>
        /// 正在存储到后端
        /// </summary>
        Storing = 1 << 0,

        /// <summary>
        /// 已存储到后端
        /// </summary>
        Stored = 1 << 1,

        /// <summary>
        /// 已处理
        /// </summary>
        Handled = 1 << 2
    }
}
