using Microsoft.AspNetCore.Http;

namespace EBank.ApiServer.Models.Output
{
    /// <summary>
    /// 已知错误情况下的 API 响应结果
    /// </summary>
    public class ApiErrorResult : ApiResult
    {
        /// <summary>
        /// 创建新 <see cref="ApiErrorResult"/> 实例。
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="messages">消息</param>
        /// <returns></returns>
        public static ApiErrorResult New(int status, object messages = null)
        {
            return new ApiErrorResult()
            {
                IsSuccessful = false,
                Status = status,
                Messages = messages
            };
        }

        /// <summary>
        /// 未找到资源，如果资源不存在，建议返回此状态码。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <param name="status">状态</param>
        /// <returns>结果</returns>
        public static ApiErrorResult NotFound(
            object messages = null,
            int status = StatusCodes.Status404NotFound) => New(status, messages);

        /// <summary>
        /// 无法验证请求，如果服务端无法处理请求，建议返回此状态码。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <param name="status">状态</param>
        /// <returns>结果</returns>
        public static ApiErrorResult BadRequest(
            object messages = null,
            int status = StatusCodes.Status400BadRequest) => New(status, messages);

        /// <summary>
        /// 未认证，如果没有提供身份信息，建议返回此状态码。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <param name="status">状态</param>
        /// <returns>结果</returns>
        public static ApiErrorResult Unauthorized(
            object messages = null,
            int status = StatusCodes.Status401Unauthorized) => New(status, messages);

        /// <summary>
        /// 未授权，如果身份认证成功，但无权访问，建议返回此状态码。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <param name="status">状态</param>
        /// <returns>结果</returns>
        public static ApiErrorResult Forbidden(
            object messages = null,
            int status = StatusCodes.Status403Forbidden) => New(status, messages);
    }
}
