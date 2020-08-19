using System;

namespace MDA.Domain.Exceptions
{
    public class DomainException : Exception
    {
        /// <summary>
        /// 业务状态码。
        /// 此机制允许服务器根据业务语义定义状态码，并传递给客户端处理，毕竟有的流程需要编排。
        /// </summary>
        public int? Code { get; set; }

        public DomainException() { }

        public DomainException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public DomainException(string message) : base(message) { }

        public DomainException(int code, string message, Exception e)
            : base(message, e)
        {
            Code = code;
        }

        public DomainException(string message, Exception e) : base(message, e) { }
    }
}
