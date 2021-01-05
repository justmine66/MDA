using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EBank.ApiServer.Models.Output
{
    /// <summary>
    /// API 响应结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <example>true</example>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 状态，通常为 HTTP 状态码，也可为业务约定代码。
        /// </summary>
        /// <example>200</example>
        public int Status { get; set; }

        /// <summary>
        /// 消息列表
        /// </summary>
        public IEnumerable<string> Messages { get; set; }

        public static OkObjectResult Ok(params string[] messages)
        {
            return new OkObjectResult(new ApiResult()
            {
                Status = StatusCodes.Status200OK,
                Messages = messages
            });
        }

        public static OkObjectResult Ok(
            IEnumerable<string> messages = null,
            int status = StatusCodes.Status200OK)
        {
            return new OkObjectResult(new ApiResult()
            {
                Status = status,
                Messages = messages
            });
        }

        public static ApiResult Accepted(
            IEnumerable<string> messages = null,
            int status = StatusCodes.Status202Accepted)
        {
            return new ApiResult()
            {
                Status = status,
                Messages = messages
            };
        }
    }

    /// <summary>
    /// API 响应结果
    /// </summary>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public class ApiResult<TPayload> : ApiResult
    {
        /// <summary>
        /// 内容载荷
        /// </summary>
        public TPayload Payload { get; set; }

        public static OkObjectResult Ok(TPayload payload = default,
            IEnumerable<string> messages = null,
            int status = StatusCodes.Status200OK)
        {
            return new OkObjectResult(new ApiResult<TPayload>()
            {
                Status = status,
                Messages = messages,
                Payload = payload
            });
        }
    }
}
