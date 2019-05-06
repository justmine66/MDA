namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个应用程序消息
    /// </summary>
    public interface IApplicationMessage
    {
        /// <summary>
        /// 业务主体标识，一般为聚合根标识。
        /// </summary>
        string BusinessPrincipalId { get; set; }
        /// <summary>
        /// 业务主体类型名称，一般为聚合根程序集完全限定名。
        /// </summary>
        string BusinessPrincipalTypeName { get; set; }
        /// <summary>
        /// 命令标识
        /// </summary>
        string CommandId { get; set; }
        /// <summary>
        /// 命令触发时间
        /// </summary>
        string CommandTime { get; set; }
        /// <summary>
        /// 命令处理时间
        /// </summary>
        string CommandProcessingTime { get; set; }
        /// <summary>
        /// 领域事件标识
        /// </summary>
        string DomainEventId { get; set; }
        /// <summary>
        /// 领域事件触发时间
        /// </summary>
        string DomainEventTime { get; set; }
        /// <summary>
        /// 领域事件处理时间
        /// </summary>
        string DomainEventProcessingTime { get; set; }
    }
}
