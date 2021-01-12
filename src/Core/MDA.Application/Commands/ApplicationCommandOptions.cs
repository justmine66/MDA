namespace MDA.Application.Commands
{
    /// <summary>
    /// 应用层命令配置项
    /// </summary>
    public class ApplicationCommandOptions
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; } = "MDA.Application.Commands";

        /// <summary>
        /// 执行配置项
        /// </summary>
        public ApplicationCommandExecutionOptions ExecutionOptions { get; set; } = new ApplicationCommandExecutionOptions();
    }

    /// <summary>
    /// 应用层命令执行配置项
    /// </summary>
    public class ApplicationCommandExecutionOptions
    {
        /// <summary>
        /// 超时，单位：秒，默认：3秒。
        /// </summary>
        public int TimeOutInSeconds { get; set; } = 3;
    }
}
