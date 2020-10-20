using System;

namespace MDA.Domain.Events
{
    public class DomainEventResult
    {
        public DomainEventResult(
            string eventId,
            DomainEventLifetime lifetime,
            DomainEventStatus status,
            string message = null,
            Exception exception = null)
        {
            EventId = eventId;
            Status = status;
            Lifetime = lifetime;
            Message = message;
            Exception = exception;
        }

        public string EventId { get; }

        public DomainEventLifetime Lifetime { get; }

        public DomainEventStatus Status { get; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public static DomainEventResult StorageSucceed(string eventId, string message = null, Exception exception = null)
            => new DomainEventResult(eventId, DomainEventLifetime.Stored, DomainEventStatus.Succeed, message, exception);

        public static DomainEventResult StorageFailed(string eventId, string message = null, Exception exception = null)
            => new DomainEventResult(eventId, DomainEventLifetime.Storing, DomainEventStatus.Failed, message, exception);

        public static DomainEventResult StorageTimeOuted(string eventId, string message = null, Exception exception = null)
            => new DomainEventResult(eventId, DomainEventLifetime.Storing, DomainEventStatus.TimeOuted, message, exception);

        public static DomainEventResult HandleSucceed(string eventId, string message = null, Exception exception = null)
            => new DomainEventResult(eventId, DomainEventLifetime.Handled, DomainEventStatus.Succeed, message, exception);
    }

    public class DomainEventResult<TEventId> : DomainEventResult
    {
        public DomainEventResult(
            TEventId eventId,
            DomainEventLifetime lifetime,
            DomainEventStatus status,
            string message = null,
            Exception exception = null)
            : base(eventId.ToString(), lifetime, status, message, exception)
        {
            EventId = eventId;
        }

        public new TEventId EventId { get; private set; }
    }

    public static class DomainEventResultExtensions
    {
        public static bool StorageSucceed(this DomainEventResult result)
            => result.Lifetime == DomainEventLifetime.Stored && 
               result.Status == DomainEventStatus.Succeed;

        public static bool StorageFailed(this DomainEventResult result)
            => result.Lifetime == DomainEventLifetime.Storing && 
               result.Status == DomainEventStatus.Failed;

        public static bool StorageTimeOuted(this DomainEventResult result)
            => result.Lifetime == DomainEventLifetime.Storing && 
               result.Status == DomainEventStatus.TimeOuted;

        public static bool HandleSucceed(this DomainEventResult result)
            => result.Lifetime == DomainEventLifetime.Handled && 
               result.Status == DomainEventStatus.Succeed;
    }
}
