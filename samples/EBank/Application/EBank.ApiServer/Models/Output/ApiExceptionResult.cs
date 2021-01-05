using EBank.ApiServer.Infrastructure.ActionResults;
using Microsoft.AspNetCore.Http;

namespace EBank.ApiServer.Models.Output
{
    /// <summary>
    /// 未知异常情况下的 API 响应结果
    /// </summary>
    public class ApiExceptionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <example>false</example>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 状态，一般为 HTTP 状态码
        /// </summary>
        /// <example>500</example>
        public int Status { get; set; }

        /// <summary>
        /// 消息列表
        /// </summary>
        public object Messages { get; set; }

        /// <summary>
        /// 开发者消息
        /// </summary>
        public object DeveloperMessage { get; set; }

        public static InternalServerErrorObjectResult InternalServerError(
            object messages = null,
            object developerMessage = null,
            int status = StatusCodes.Status500InternalServerError)
        {
            return new InternalServerErrorObjectResult(new ApiExceptionResult()
            {
                Status = status,
                Messages = messages,
                DeveloperMessage = developerMessage
            });
        }
    }
}
