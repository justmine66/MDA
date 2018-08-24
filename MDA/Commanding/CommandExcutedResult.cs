using System;

namespace MDA.Commanding
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class CommandExcutedResult
    {
        public CommandExcutedStatus ExcutedStatus { get; private set; }
        public string CommandId { get; private set; }
        public string Result { get; private set; }
        public string ResultType { get; private set; }

        public override string ToString()
        {
            return string.Format("CommandExcutedResult [CommandId={0},Status={1},Result={2},ResultType={3}]",
                CommandId,
                Enum.GetName(typeof(CommandExcutedStatus), ExcutedStatus),
                Result,
                ResultType);
        }
    }
    /// <summary>
    /// 命令执行状态
    /// </summary>
    public enum CommandExcutedStatus
    {
        None = 0,
        Success = 1,
        NothingChanged = 2,
        Failed = 3
    }
}
