namespace MDA.Domain.Commands
{
    public class DomainCommandResult
    {
        public DomainCommandResult(string commandId, DomainCommandStatus status, object result = null)
        {
            CommandId = commandId;
            Status = status;
            Result = result;
        }

        public string CommandId { get; private set; }

        public DomainCommandStatus Status { get; private set; }

        public object Result { get; private set; }

        public static DomainCommandResult Success(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.Success, result);

        public static DomainCommandResult Failed(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.Failed, result);

        public static DomainCommandResult TimeOut(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.TimeOut, result);
    }

    public class DomainCommandResult<TResult> : DomainCommandResult
    {
        public DomainCommandResult(string commandId, DomainCommandStatus status, TResult result = default)
            : base(commandId, status, result)
        {
            Result = result;
        }

        public new TResult Result { get; private set; }

        public static DomainCommandResult Success(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.Success, result);

        public static DomainCommandResult Failed(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.Failed, result);

        public static DomainCommandResult TimeOut(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.TimeOut, result);
    }

    public class DomainCommandResult<TResult, TCommandId> : DomainCommandResult<TResult>
    {
        public DomainCommandResult(TCommandId commandId, DomainCommandStatus status, TResult result = default)
            : base(commandId.ToString(), status, result)
        {
            CommandId = commandId;
        }

        public new TCommandId CommandId { get; private set; }
    }
}
