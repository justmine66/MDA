using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBank.ApiServer.Models.Output
{
    /// <summary>
    /// 已知错误情况下的 API 响应结果
    /// </summary>
    public class ApiErrorResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <example>false</example>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 状态，一般为 HTTP 状态码
        /// </summary>
        /// <example>400</example>
        public int Status { get; set; }

        /// <summary>
        /// 消息列表
        /// </summary>
        public object Messages { get; set; }

        public static NotFoundObjectResult NotFound(
            object messages = null,
            int status = StatusCodes.Status404NotFound)
        {
            return new NotFoundObjectResult(new ApiErrorResult()
            {
                Status = status,
                Messages = messages
            });
        }

        public static BadRequestObjectResult BadRequest(
            object messages = null,
            int status = StatusCodes.Status400BadRequest)
        {
            return new BadRequestObjectResult(new ApiErrorResult()
            {
                Status = status,
                Messages = messages
            });
        }
    }
}
