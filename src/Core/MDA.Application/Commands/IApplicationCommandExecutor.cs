using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令执行器
    /// </summary>
    public interface IApplicationCommandExecutor
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="returnScheme">返回方案，默认：当领域命令被处理后，返回执行结果。</param>
        /// <returns>结果</returns>
        ApplicationCommandResult ExecuteCommand(
            IApplicationCommand command, 
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled);

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="TPayload">内容载荷类型</typeparam>
        /// <param name="command">命令</param>
        /// <param name="returnScheme">返回方案，默认：当领域命令被处理后，返回执行结果。</param>
        /// <returns>结果</returns>
        ApplicationCommandResult<TPayload> ExecuteCommand<TPayload>(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="token">取消令牌</param>
        /// <returns>结果</returns>
        Task<ApplicationCommandResult> ExecuteCommandAsync(
            IApplicationCommand command,
            CancellationToken token = default);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="returnScheme">返回方案，默认：当领域命令被处理后，返回执行结果。</param>
        /// <param name="token">取消令牌</param>
        /// <returns>结果</returns>
        Task<ApplicationCommandResult> ExecuteCommandAsync(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default);

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="TPayload">内容载荷类型</typeparam>
        /// <param name="command">命令</param>
        /// <param name="token">取消令牌</param>
        /// <returns>结果</returns>
        Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(
            IApplicationCommand command,
            CancellationToken token = default);

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="TPayload">内容载荷类型</typeparam>
        /// <param name="command">命令</param>
        /// <param name="returnScheme">返回方案，默认：当领域命令被处理后，返回执行结果。</param>
        /// <param name="token">取消令牌</param>
        /// <returns>结果</returns>
        Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default);
    }
}
