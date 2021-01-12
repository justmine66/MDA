using Microsoft.AspNetCore.Http;

namespace EBank.ApiServer.Models.Output
{
    /// <summary>
    /// 未知异常情况下的 API 响应结果
    /// </summary>
    public class ApiExceptionResult : ApiErrorResult
    {
        /// <summary>
        /// 开发者消息
        /// </summary>
        public object DeveloperMessages { get; set; }

        /// <summary>
        /// 创建新 <see cref="ApiExceptionResult"/> 实例。
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="messages">消息</param>
        /// <param name="developerMessages">消息，受众为开发环境的开发者。</param>
        /// <returns>结果</returns>
        public static ApiExceptionResult New(int status, object messages = null, object developerMessages = null)
        {
            return new ApiExceptionResult()
            {
                IsSuccessful = false,
                Status = status,
                Messages = messages,
                DeveloperMessages = developerMessages
            };
        }

        /// <summary>
        /// 服务器端错误
        /// </summary>
        /// <param name="messages">消息，受众为对接者。</param>
        /// <param name="developerMessages">消息，受众为开发环境的开发者。</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static ApiExceptionResult InternalServerError(
            object messages = null,
            object developerMessages = null,
            int status = StatusCodes.Status500InternalServerError) => New(status, messages, developerMessages);

        /// <summary>
        /// 上游网关超时
        /// </summary>
        /// <param name="messages">消息，受众为对接者。</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static ApiExceptionResult GatewayTimeout(
            object messages = null,
            int status = StatusCodes.Status504GatewayTimeout) => New(status, messages);
    }
}
