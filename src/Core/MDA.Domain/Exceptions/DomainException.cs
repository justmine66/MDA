using System;

namespace MDA.Domain.Exceptions
{
    /// <summary>
    /// 领域异常
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// 业务状态码。
        /// 此机制允许服务器根据业务语义定义状态码，并传递给客户端处理，以协调端到端的流程编排。
        /// </summary>
        public int Code { get; set; }

        public new string Message { get; set; }

        public DomainException() => Init();

        public DomainException(int code, string message)
            : base(message)
        {
            Code = code;
            Init();
        }

        public DomainException(string message) : base(message)
        {
            Message = message;
            Init();
        }

        public DomainException(int code, string message, Exception e)
            : base(message, e)
        {
            Code = code;
            Init();
        }

        public DomainException(string message, Exception e) : base(message, e)
        {
            Message = message;
            Init();
        }

        public DomainException(Exception e) : base(string.Empty, e)
        {
            Message = string.Empty;

            Init();
        }

        /// <summary>
        /// 应用层命令标识
        /// </summary>
        public string ApplicationCommandId { get; set; }

        /// <summary>
        /// 应用层命令类型
        /// </summary>
        public string ApplicationCommandType { get; set; }

        /// <summary>
        /// 应用层命令回复方案
        /// </summary>
        public ApplicationCommandReplySchemes ApplicationCommandReplyScheme { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        public string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名称。
        /// </summary>
        public string DomainCommandType { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        public string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名称
        /// </summary>
        public string AggregateRootType { get; set; }

        /// <summary>
        /// 有效载荷
        /// </summary>
        public object Payload { get; set; }

        public string Id { get; set; }
        public long Timestamp { get; set; }
        public string Topic { get; set; }
        public int PartitionKey { get; set; }

        protected void Init()
        {
            Id = Guid.NewGuid().ToString("N");
            Topic = DomainDefaults.Topics.Exception;
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            ApplicationCommandReplyScheme = ApplicationCommandReplySchemes.None;
        }
    }
}
