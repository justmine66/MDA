namespace MDA.StateBackend.MySql
{
    public class DomainEventRecord
    {
        /// <summary>
        /// 领域事件标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 领域事件版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 领域事件类型完全限定名
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        public string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名
        /// </summary>
        public string DomainCommandTypeFullName { get; set; }

        /// <summary>
        /// 领域命令版本
        /// </summary>
        public int DomainCommandVersion { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名
        /// </summary>
        public string AggregateRootTypeFullName { get; set; }

        /// <summary>
        /// 聚合根版本
        /// </summary>
        public int AggregateRootVersion { get; set; }

        /// <summary>
        /// 有效载荷
        /// </summary>
        public byte[] Payload { get; set; }
    }
}
