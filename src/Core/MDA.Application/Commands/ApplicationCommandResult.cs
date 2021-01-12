namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令结果
    /// </summary>
    public class ApplicationCommandResult
    {
        /// <summary>
        /// 初始化 <see cref="ApplicationCommandResult"/> 对象。
        /// </summary>
        /// <param name="commandId">命令标识</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="status">状态</param>
        /// <param name="payload">内容载荷</param>
        public ApplicationCommandResult(
            string commandId,
            string commandType,
            ApplicationCommandStatus status,
            object payload = null)
        {
            CommandId = commandId;
            Status = status;
            CommandType = commandType;
            Payload = payload;
        }

        /// <summary>
        /// 命令标识
        /// </summary>
        public string CommandId { get; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public string CommandType { get; }

        /// <summary>
        /// 状态
        /// </summary>
        public ApplicationCommandStatus Status { get; }

        /// <summary>
        /// 内容载荷
        /// </summary>
        public object Payload { get; }

        public static ApplicationCommandResult Succeed(string commandId, string commandType, object result = null)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.Succeed, result);

        public static ApplicationCommandResult Failed(string commandId, string commandType, object result = null)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.Failed, result);

        public static ApplicationCommandResult TimeOuted(string commandId, string commandType, object result = null)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.TimeOuted, result);

        public static ApplicationCommandResult Canceled(string commandId, string commandType, object result = null)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.Canceled, result);
    }

    /// <summary>
    /// 应用层命令结果
    /// </summary>
    /// <typeparam name="TPayload">内容载荷类型</typeparam>
    public class ApplicationCommandResult<TPayload> : ApplicationCommandResult
    {
        /// <summary>
        /// 初始化 <see cref="ApplicationCommandResult{TPayload}"/> 对象。
        /// </summary>
        /// <param name="commandId">命令标识</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="status">状态</param>
        /// <param name="payload">内容载荷</param>
        public ApplicationCommandResult(
            string commandId,
            string commandType,
            ApplicationCommandStatus status,
            TPayload payload = default)
            : base(commandId, commandType, status, payload)
        {
            Payload = payload;
        }

        /// <summary>
        /// 内容载荷
        /// </summary>
        public new TPayload Payload { get; }

        public static ApplicationCommandResult Succeed(string commandId, string commandType, TPayload result = default)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.Succeed, result);

        public static ApplicationCommandResult Failed(string commandId, string commandType, TPayload result = default)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.Failed, result);

        public static ApplicationCommandResult TimeOuted(string commandId, string commandType, TPayload result = default)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.TimeOuted, result);

        public static ApplicationCommandResult Canceled(string commandId, string commandType, TPayload result = default)
            => new ApplicationCommandResult(commandId, commandType, ApplicationCommandStatus.Canceled, result);
    }

    /// <summary>
    /// 应用层命令结果
    /// </summary>
    /// <typeparam name="TPayload">内容载荷类型</typeparam>
    /// <typeparam name="TCommandId">命令标识类型</typeparam>
    public class ApplicationCommandResult<TPayload, TCommandId> : ApplicationCommandResult<TPayload>
    {
        /// <summary>
        /// 初始化 <see cref="ApplicationCommandResult{TPayload, TCommandId}"/> 对象。
        /// </summary>
        /// <param name="commandId">命令标识</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="status">状态</param>
        /// <param name="payload">内容载荷</param>
        public ApplicationCommandResult(
            TCommandId commandId,
            string commandType,
            ApplicationCommandStatus status,
            TPayload payload = default)
            : base(commandId.ToString(), commandType, status, payload)
        {
            CommandId = commandId;
        }

        /// <summary>
        /// 命令标识
        /// </summary>
        public new TCommandId CommandId { get; private set; }
    }
}
