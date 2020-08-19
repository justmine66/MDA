namespace MDA.Domain.Events
{
    public class DomainEventResult
    {
        public DomainEventResult(string eventId, DomainEventStatus status, object result = null)
        {
            EventId = eventId;
            Status = status;
            Result = result;
        }

        public string EventId { get; private set; }

        public DomainEventStatus Status { get; private set; }

        public object Result { get; private set; }

        public static DomainEventResult Success(string eventId, object result = null) => new DomainEventResult(eventId, DomainEventStatus.Success, result);

        public static DomainEventResult Failed(string eventId, object result = null) => new DomainEventResult(eventId, DomainEventStatus.Failed, result);

        public static DomainEventResult TimeOut(string eventId, object result = null) => new DomainEventResult(eventId, DomainEventStatus.TimeOut, result);
    }

    public class DomainEventResult<TResult> : DomainEventResult
    {
        public DomainEventResult(string eventId, DomainEventStatus status, TResult result = default)
            : base(eventId, status, result)
        {
            Result = result;
        }

        public new TResult Result { get; private set; }

        public static DomainEventResult Success(string eventId, TResult result = default) => new DomainEventResult(eventId, DomainEventStatus.Success, result);

        public static DomainEventResult Failed(string eventId, TResult result = default) => new DomainEventResult(eventId, DomainEventStatus.Failed, result);

        public static DomainEventResult TimeOut(string eventId, TResult result = default) => new DomainEventResult(eventId, DomainEventStatus.TimeOut, result);
    }

    public class DomainEventResult<TResult, TEventId> : DomainEventResult<TResult>
    {
        public DomainEventResult(TEventId eventId, DomainEventStatus status, TResult result = default)
            : base(eventId.ToString(), status, result)
        {
            EventId = eventId;
        }

        public new TEventId EventId { get; private set; }
    }
}
