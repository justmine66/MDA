namespace MDA.Application.Commands
{
    public class ApplicationCommandResult
    {
        public ApplicationCommandResult(string commandId, ApplicationCommandStatus status, object result = null)
        {
            CommandId = commandId;
            Status = status;
            Result = result;
        }

        public string CommandId { get; private set; }

        public ApplicationCommandStatus Status { get; private set; }

        public object Result { get; private set; }

        public static ApplicationCommandResult Success(string commandId, object result = null) => new ApplicationCommandResult(commandId, ApplicationCommandStatus.Successed, result);

        public static ApplicationCommandResult Failed(string commandId, object result = null) => new ApplicationCommandResult(commandId, ApplicationCommandStatus.Failed, result);

        public static ApplicationCommandResult TimeOut(string commandId, object result = null) => new ApplicationCommandResult(commandId, ApplicationCommandStatus.Timeouted, result);
    }

    public class ApplicationCommandResult<TResult> : ApplicationCommandResult
    {
        public ApplicationCommandResult(string commandId, ApplicationCommandStatus status, TResult result = default)
            : base(commandId, status, result)
        {
            Result = result;
        }

        public new TResult Result { get; private set; }

        public static ApplicationCommandResult Success(string commandId, TResult result = default) => new ApplicationCommandResult(commandId, ApplicationCommandStatus.Successed, result);

        public static ApplicationCommandResult Failed(string commandId, TResult result = default) => new ApplicationCommandResult(commandId, ApplicationCommandStatus.Failed, result);

        public static ApplicationCommandResult TimeOut(string commandId, TResult result = default) => new ApplicationCommandResult(commandId, ApplicationCommandStatus.Timeouted, result);
    }

    public class ApplicationCommandResult<TResult, TCommandId> : ApplicationCommandResult<TResult>
    {
        public ApplicationCommandResult(TCommandId commandId, ApplicationCommandStatus status, TResult result = default)
            : base(commandId.ToString(), status, result)
        {
            CommandId = commandId;
        }

        public new TCommandId CommandId { get; private set; }
    }
}
