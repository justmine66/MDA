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

        public string CommandId { get; }

        public DomainCommandStatus Status { get; }

        public object Result { get; }

        public static DomainCommandResult Published(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.Published, result);

        public static DomainCommandResult Succeed(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.Succeed, result);

        public static DomainCommandResult Failed(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.Failed, result);

        public static DomainCommandResult TimeOuted(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.TimeOuted, result);

        public static DomainCommandResult Canceled(string commandId, object result = null) => new DomainCommandResult(commandId, DomainCommandStatus.Canceled, result);
    }

    public class DomainCommandResult<TResult> : DomainCommandResult
    {
        public DomainCommandResult(string commandId, DomainCommandStatus status, TResult result = default)
            : base(commandId, status, result)
        {
            Result = result;
        }

        public new TResult Result { get; private set; }

        public static DomainCommandResult Succeed(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.Succeed, result);

        public static DomainCommandResult Failed(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.Failed, result);

        public static DomainCommandResult TimeOuted(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.TimeOuted, result);

        public static DomainCommandResult Canceled(string commandId, TResult result = default) => new DomainCommandResult(commandId, DomainCommandStatus.Canceled, result);
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
