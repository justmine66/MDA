using System;

namespace MDA.Domain.Commands
{
    public class DomainCommandResult
    {
        public DomainCommandResult(
            string commandId,
            DomainCommandStatus status,
            string message = null,
            Exception exception = null)
        {
            CommandId = commandId;
            Status = status;
            Message = message;
            Exception = exception;
        }

        public string CommandId { get; }

        public DomainCommandStatus Status { get; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public static DomainCommandResult Published(string commandId, string message = null, Exception exception = null)
            => new DomainCommandResult(commandId, DomainCommandStatus.Published, message, exception);

        public static DomainCommandResult Succeed(string commandId, string message = null, Exception exception = null)
            => new DomainCommandResult(commandId, DomainCommandStatus.Succeed, message, exception);

        public static DomainCommandResult Failed(string commandId, string message = null, Exception exception = null)
            => new DomainCommandResult(commandId, DomainCommandStatus.Failed, message, exception);

        public static DomainCommandResult TimeOuted(string commandId, string message = null, Exception exception = null)
            => new DomainCommandResult(commandId, DomainCommandStatus.TimeOuted, message, exception);

        public static DomainCommandResult Canceled(string commandId, string message = null, Exception exception = null)
            => new DomainCommandResult(commandId, DomainCommandStatus.Canceled, message, exception);
    }

    public class DomainCommandResult<TCommandId> : DomainCommandResult
    {
        public DomainCommandResult(
            TCommandId commandId, 
            DomainCommandStatus status, 
            string message = null, 
            Exception exception = null)
            : base(commandId.ToString(), status, message, exception)
        {
            CommandId = commandId;
        }

        public new TCommandId CommandId { get; private set; }
    }

    public static class DomainCommandResultExtensions
    {
        public static bool Succeed(this DomainCommandResult result) 
            => result.Status == DomainCommandStatus.Succeed;

        public static bool Failed(this DomainCommandResult result) 
            => result.Status == DomainCommandStatus.Failed;

        public static bool Canceled(this DomainCommandResult result)
            => result.Status == DomainCommandStatus.Canceled;

        public static bool TimeOuted(this DomainCommandResult result) 
            => result.Status == DomainCommandStatus.TimeOuted;

        public static bool Published(this DomainCommandResult result) 
            => result.Status == DomainCommandStatus.Published;
    }
}
