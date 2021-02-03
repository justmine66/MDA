namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令结果监听器
    /// </summary>
    public interface IApplicationCommandResultListener
    {
        void AddExecutingPromise(ApplicationCommandExecutionPromise promise);

        void AddExecutingPromise<TPayload>(ApplicationCommandExecutionPromise<TPayload> promise);
    }
}
