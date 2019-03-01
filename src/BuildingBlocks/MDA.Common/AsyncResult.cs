namespace MDA.Common
{
    /// <summary>
    /// 表示一个异步结果。
    /// </summary>
    public class AsyncResult
    {
        /// <summary>
        /// 表示一个成功的异步结果。
        /// </summary>
        public static AsyncResult Success = new AsyncResult(AsyncStatus.Success, null);

        /// <summary>
        /// 状态
        /// </summary>
        public AsyncStatus Status { get; private set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 初始化一个 <see cref="AsyncResult"/> 实例。
        /// </summary>
        /// <param name="status">状态。初始化 <see cref="Status"/> 属性。</param>
        public AsyncResult(AsyncStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 初始化一个 <see cref="AsyncResult"/> 实例。
        /// </summary>
        /// <param name="status">状态。初始化 <see cref="Status"/> 属性。</param>
        /// <param name="errorMessage">错误消息。初始化 <see cref="ErrorMessage"/> 属性。</param>
        public AsyncResult(AsyncStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }
    }
    /// <summary>
    /// 表示一个泛型异步结果。
    /// </summary>
    /// <typeparam name="TDataType">数据类型</typeparam>
    public class AsyncResult<TState> : AsyncResult
    {
        /// <summary>
        /// 初始化 <see cref="AsyncResult{TData}"/> 实例。
        /// </summary>
        /// <param name="status">状态。初始化 <see cref="Status"/> 属性。</param>
        public AsyncResult(AsyncStatus status)
            : this(status, null, default(TState))
        {

        }

        /// <summary>
        /// 初始化 <see cref="AsyncResult{TData}"/> 实例。
        /// </summary>
        /// <param name="status">状态。初始化 <see cref="Status"/> 属性。</param>
        /// <param name="stateInfo">状态信息。初始化 <see cref="StateInfo"/> 属性。</param>
        public AsyncResult(AsyncStatus status, TState stateInfo)
            : this(status, null, stateInfo)
        {

        }
        /// <summary>
        /// 初始化 <see cref="AsyncResult{TData}"/> 实例。
        /// </summary>
        /// <param name="status">状态。初始化 <see cref="Status"/> 属性。</param>
        /// <param name="errorMessage">错误消息。初始化 <see cref="ErrorMessage"/> 属性。</param>
        public AsyncResult(AsyncStatus status, string errorMessage)
            : base(status, errorMessage)
        {
        }

        /// <summary>
        /// 初始化 <see cref="AsyncResult{TData}"/> 实例。
        /// </summary>
        /// <param name="status">状态。初始化 <see cref="Status"/> 属性。</param>
        /// <param name="errorMessage">错误消息。初始化 <see cref="ErrorMessage"/> 属性。</param>
        /// <param name="stateInfo">状态信息。初始化 <see cref="StateInfo"/> 属性。</param>
        public AsyncResult(AsyncStatus status, string errorMessage, TState stateInfo)
            : base(status, errorMessage)
        {
            StateInfo = stateInfo;
        }

        /// <summary>
        /// 状态信息
        /// </summary>
        public TState StateInfo { get; private set; }
    }
    /// <summary>
    /// 异步状态
    /// </summary>
    public enum AsyncStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        /// <summary>
        /// 超时
        /// </summary>
        Timeouted
    }
}
