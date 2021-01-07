using EBank.ApiServer.Models.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBank.ApiServer.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class AbstractController : ControllerBase
    {
        /// <summary>
        /// 创建一个 <see cref="OkObjectResult"/> 对象。
        /// </summary>
        /// <param name="value">内容载荷</param>
        /// <returns>结果</returns>
        [NonAction]
        public override OkObjectResult Ok(object value) => Ok(value);

        /// <summary>
        /// 创建一个 <see cref="OkObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public OkObjectResult Ok(params string[] messages) => Ok<object>(null, StatusCodes.Status200OK, messages);

        /// <summary>
        /// 创建一个 <see cref="OkObjectResult"/> 对象。
        /// </summary>
        /// <typeparam name="TPayload">内容载荷内心</typeparam>
        /// <param name="payload">内容载荷</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public OkObjectResult Ok<TPayload>(TPayload payload, params string[] messages) => Ok(payload, StatusCodes.Status200OK, messages);

        /// <summary>
        /// 创建一个 <see cref="OkObjectResult"/> 对象。
        /// </summary>
        /// <typeparam name="TPayload">内容载荷内心</typeparam>
        /// <param name="payload">内容载荷</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public OkObjectResult Ok<TPayload>(TPayload payload, int status = StatusCodes.Status200OK, params string[] messages)
        {
            return base.Ok(ApiResult<TPayload>.Ok(payload, status, messages));
        }

        /// <summary>
        /// 创建一个 <see cref="AcceptedResult"/> 对象。
        /// </summary>
        /// <param name="value">内容载荷</param>
        /// <returns>结果</returns>
        [NonAction]
        public override AcceptedResult Accepted(object value) => Accepted(value);

        /// <summary>
        /// 创建一个 <see cref="AcceptedResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public AcceptedResult Accepted(params string[] messages) => Accepted<object>(null, StatusCodes.Status202Accepted, messages);

        /// <summary>
        /// 创建一个 <see cref="AcceptedResult"/> 对象。
        /// </summary>
        /// <typeparam name="TPayload">内容载荷内心</typeparam>
        /// <param name="payload">内容载荷</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public AcceptedResult Accepted<TPayload>(TPayload payload, params string[] messages) => Accepted(payload, StatusCodes.Status200OK, messages);

        /// <summary>
        /// 创建一个 <see cref="AcceptedResult"/> 对象。
        /// </summary>
        /// <typeparam name="TPayload">内容载荷内心</typeparam>
        /// <param name="payload">内容载荷</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public AcceptedResult Accepted<TPayload>(TPayload payload, int status = StatusCodes.Status202Accepted, params string[] messages)
        {
            return base.Accepted(ApiResult<TPayload>.Accepted(payload, status, messages));
        }

        /// <summary>
        /// 创建一个 <see cref="NotFoundObjectResult"/> 对象。
        /// </summary>
        /// <param name="value">内容载荷</param>
        /// <returns>结果</returns>
        [NonAction]
        public override NotFoundObjectResult NotFound(object value)
        {
            return base.NotFound(ApiErrorResult.NotFound(value));
        }

        /// <summary>
        /// 创建一个 <see cref="NotFoundObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public NotFoundObjectResult NotFound(params string[] messages) => NotFound(StatusCodes.Status202Accepted, messages);

        /// <summary>
        /// 创建一个 <see cref="NotFoundObjectResult"/> 对象。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public NotFoundObjectResult NotFound(int status = StatusCodes.Status404NotFound, params string[] messages)
        {
            return base.NotFound(ApiErrorResult.NotFound(messages, status));
        }

        /// <summary>
        /// 创建一个 <see cref="BadRequestObjectResult"/> 对象。
        /// </summary>
        /// <param name="value">内容载荷</param>
        /// <returns>结果</returns>
        [NonAction]
        public override BadRequestObjectResult BadRequest(object value)
        {
            return base.BadRequest(ApiErrorResult.BadRequest(value));
        }

        /// <summary>
        /// 创建一个 <see cref="BadRequestObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public BadRequestObjectResult BadRequest(params string[] messages) => BadRequest(StatusCodes.Status202Accepted, messages);

        /// <summary>
        /// 创建一个 <see cref="BadRequestObjectResult"/> 对象。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public BadRequestObjectResult BadRequest(int status = StatusCodes.Status400BadRequest, params string[] messages)
        {
            return base.BadRequest(ApiErrorResult.BadRequest(messages, status));
        }
    }
}
