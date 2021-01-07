using Microsoft.AspNetCore.Http;
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
        public bool IsSuccessful { get; set; } = true;

        /// <summary>
        /// 状态，通常为 HTTP 状态码，也可为业务约定代码。
        /// </summary>
        /// <example>200</example>
        public int Status { get; set; }

        /// <summary>
        /// 消息列表
        /// </summary>
        public object Messages { get; set; }

        /// <summary>
        /// 创建新 <see cref="ApiResult"/> 实例。
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="messages">消息</param>
        /// <returns></returns>
        public static ApiResult New(int status, IEnumerable<string> messages = null)
        {
            return new ApiResult()
            {
                Status = status,
                Messages = messages
            };
        }

        /// <summary>
        /// 创建新 <see cref="ApiResult"/> 实例。
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="messages">消息</param>
        /// <returns></returns>
        public static ApiResult New(int status, params string[] messages)
        {
            return new ApiResult()
            {
                Status = status,
                Messages = messages
            };
        }

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult Ok(int status = StatusCodes.Status200OK, IEnumerable<string> messages = null) => New(status, messages);

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult Ok(int status = StatusCodes.Status200OK, params string[] messages) => New(status, messages);

        /// <summary>
        /// 请求已接受，如果是前后端协调的异步处理，以便实现读写分离，提高性能，建议返回这个状态码，比如：银行转账，先返回已接受，客户端再轮询结果。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult Accepted(int status = StatusCodes.Status202Accepted, IEnumerable<string> messages = null) => New(status, messages);

        /// <summary>
        /// 请求已接受，如果是前后端协调的异步处理，以便实现读写分离，提高性能，建议返回这个状态码，比如：银行转账，先返回已接受，客户端再轮询结果。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult Accepted(int status = StatusCodes.Status202Accepted, params string[] messages) => New(status, messages);

        /// <summary>
        /// 请求已成功创建了资源，如果是新增数据，建议返回这个状态码。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult Created(int status = StatusCodes.Status201Created, params string[] messages) => New(status, messages);

        /// <summary>
        /// 请求已成功创建了资源，如果是新增数据，建议返回这个状态码。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult Created(int status = StatusCodes.Status201Created, IEnumerable<string> messages = null) => New(status, messages);

        /// <summary>
        /// 请求成功，但没有资源返回，如果是删除数据，建议返回这个状态码。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult NoContent(int status = StatusCodes.Status204NoContent, params string[] messages) => New(status, messages);

        /// <summary>
        /// 请求成功，但没有资源返回，如果是删除数据，建议返回这个状态码。
        /// </summary>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult NoContent(int status = StatusCodes.Status204NoContent, IEnumerable<string> messages = null) => New(status, messages);
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

        /// <summary>
        /// 创建新 <see cref="ApiResult{TPayload}"/> 实例。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态</param>
        /// <param name="messages">消息</param>
        /// <returns></returns>
        public static ApiResult<TPayload> New(TPayload payload, int status, IEnumerable<string> messages = null)
        {
            return new ApiResult<TPayload>()
            {
                Payload = payload,
                Status = status,
                Messages = messages
            };
        }

        /// <summary>
        /// 创建新 <see cref="ApiResult"/> 实例。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态</param>
        /// <param name="messages">消息</param>
        /// <returns></returns>
        public static ApiResult<TPayload> New(TPayload payload, int status, params string[] messages)
        {
            return new ApiResult<TPayload>()
            {
                Payload = payload,
                Status = status,
                Messages = messages
            };
        }

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> Ok(TPayload payload = default, int status = StatusCodes.Status200OK, IEnumerable<string> messages = null) => New(payload, status, messages);

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> Ok(TPayload payload = default, int status = StatusCodes.Status200OK, params string[] messages) => New(payload, status, messages);

        /// <summary>
        /// 请求已接受，如果是前后端协调的异步处理，以便实现读写分离，提高性能，建议返回这个状态码，比如：银行转账，先返回已接受，客户端再轮询结果。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> Accepted(TPayload payload = default, int status = StatusCodes.Status202Accepted, IEnumerable<string> messages = null) => New(payload, status, messages);

        /// <summary>
        /// 请求已接受，如果是前后端协调的异步处理，以便实现读写分离，提高性能，建议返回这个状态码，比如：银行转账，先返回已接受，客户端再轮询结果。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> Accepted(TPayload payload = default, int status = StatusCodes.Status202Accepted, params string[] messages) => New(payload, status, messages);

        /// <summary>
        /// 请求已成功创建了资源，如果是新增数据，建议返回这个状态码。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> Created(TPayload payload = default, int status = StatusCodes.Status201Created, IEnumerable<string> messages = null) => New(payload, status, messages);

        /// <summary>
        /// 请求已成功创建了资源，如果是新增数据，建议返回这个状态码。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> Created(TPayload payload = default, int status = StatusCodes.Status201Created, params string[] messages) => New(payload, status, messages);

        /// <summary>
        /// 请求成功，但没有资源返回，如果是删除数据，建议返回这个状态码。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> NoContent(TPayload payload = default, int status = StatusCodes.Status204NoContent, IEnumerable<string> messages = null) => New(payload, status, messages);

        /// <summary>
        /// 请求成功，但没有资源返回，如果是删除数据，建议返回这个状态码。
        /// </summary>
        /// <param name="payload">内容载荷类型</param>
        /// <param name="status">状态，通常为 HTTP 状态码，也可为业务约定代码。</param>
        /// <param name="messages"></param>
        /// <returns>结果</returns>
        public static ApiResult<TPayload> NoContent(TPayload payload = default, int status = StatusCodes.Status204NoContent, params string[] messages) => New(payload, status, messages);
    }
}
