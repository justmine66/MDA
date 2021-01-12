using MDA.MessageBus;
using System;

namespace MDA.Domain.Exceptions
{
    /// <summary>
    /// 领域异常消息，用于传输端到端的异常消息。
    /// </summary>
    public interface IDomainExceptionMessage : IMessage
    {
        /// <summary>
        /// 应用层命令标识
        /// </summary>
        string ApplicationCommandId { get; set; }

        /// <summary>
        /// 应用层命令类型
        /// </summary>
        string ApplicationCommandType { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名称。
        /// </summary>
        string DomainCommandType { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名称
        /// </summary>
        string AggregateRootType { get; set; }

        /// <summary>
        /// 业务状态码。
        /// 此机制允许服务器根据业务语义定义状态码，并传递给客户端处理，以协调端到端的流程编排。
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; set; }
    }

    public class DomainExceptionMessage : IDomainExceptionMessage
    {
        public DomainExceptionMessage()
        {
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public string Id { get; set; }
        public long Timestamp { get; set; }
        public string Topic { get; set; }
        public int PartitionKey { get; set; }
        public string ApplicationCommandId { get; set; }
        public string ApplicationCommandType { get; set; }
        public string DomainCommandId { get; set; }
        public string DomainCommandType { get; set; }
        public string AggregateRootId { get; set; }
        public string AggregateRootType { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
