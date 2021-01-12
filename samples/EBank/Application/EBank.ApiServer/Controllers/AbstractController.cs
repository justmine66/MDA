using EBank.ApiServer.Infrastructure.ActionResults;
using EBank.ApiServer.Models.Output;
using MDA.Application.Commands;
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
        public AcceptedResult Accepted(params string[] messages) => Accepted<object>(null, StatusCodes.Status200OK, messages);

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
        public NotFoundObjectResult NotFound(params string[] messages) => NotFound(StatusCodes.Status404NotFound, messages);

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
        public BadRequestObjectResult BadRequest(params string[] messages) => BadRequest(StatusCodes.Status400BadRequest, messages);

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

        /// <summary>
        /// 创建一个 <see cref="InternalServerErrorObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public InternalServerErrorObjectResult Timeout(object messages)
        {
            var result = ApiExceptionResult.GatewayTimeout(messages);

            return new InternalServerErrorObjectResult(result, result.Status);
        }

        /// <summary>
        /// 创建一个 <see cref="InternalServerErrorObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public InternalServerErrorObjectResult Timeout(params string[] messages)
        {
            var result = ApiExceptionResult.GatewayTimeout(messages);

            return new InternalServerErrorObjectResult(result, result.Status);
        }

        /// <summary>
        /// 创建一个 <see cref="InternalServerErrorObjectResult"/> 对象。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public InternalServerErrorObjectResult Timeout(int status = StatusCodes.Status504GatewayTimeout, params string[] messages)
        {
            var result = ApiExceptionResult.GatewayTimeout(messages, status);

            return new InternalServerErrorObjectResult(result, result.Status);
        }

        /// <summary>
        /// 创建一个 <see cref="InternalServerErrorObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public InternalServerErrorObjectResult ServerError(object messages)
        {
            var result = ApiExceptionResult.GatewayTimeout(messages);

            return new InternalServerErrorObjectResult(result);
        }

        /// <summary>
        /// 创建一个 <see cref="InternalServerErrorObjectResult"/> 对象。
        /// </summary>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public InternalServerErrorObjectResult ServerError(params string[] messages)
        {
            var result = ApiExceptionResult.GatewayTimeout(messages);

            return new InternalServerErrorObjectResult(result);
        }

        /// <summary>
        /// 创建一个 <see cref="InternalServerErrorObjectResult"/> 对象。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages">消息</param>
        /// <returns>结果</returns>
        [NonAction]
        public InternalServerErrorObjectResult ServerError(int status = StatusCodes.Status500InternalServerError, params string[] messages)
        {
            var result = ApiExceptionResult.InternalServerError(messages);

            return new InternalServerErrorObjectResult(result);
        }

        /// <summary>
        /// 按照应用层命令执行状态装备API结果
        /// </summary>
        /// <param name="result">应用层命令</param>
        /// <returns>API结果</returns>
        [NonAction]
        public IActionResult ExecutionResult(ApplicationCommandResult result)
        {
            var payload = result.Payload;

            switch (result.Status)
            {
                case ApplicationCommandStatus.Succeed:
                    return Ok(payload);
                case ApplicationCommandStatus.Failed:
                    return BadRequest(payload);
                case ApplicationCommandStatus.TimeOuted:
                    return Timeout(payload);
                default:
                    return ServerError(payload);
            }
        }
    }
}
