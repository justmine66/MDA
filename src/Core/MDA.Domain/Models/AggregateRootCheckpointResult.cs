using System;

namespace MDA.Domain.Models
{
    public class AggregateRootCheckpointResult
    {
        public AggregateRootCheckpointResult(
            string aggregateRootId,
            AggregateRootCheckpointLifetime lifetime,
            AggregateRootCheckpointStatus status,
            string message = null,
            Exception exception = null)
        {
            AggregateRootId = aggregateRootId;
            Status = status;
            Lifetime = lifetime;
            Message = message;
            Exception = exception;
        }

        public string AggregateRootId { get; }

        public AggregateRootCheckpointLifetime Lifetime { get; }

        public AggregateRootCheckpointStatus Status { get; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public static AggregateRootCheckpointResult StorageSucceed(string aggregateRootId, string message = null, Exception exception = null)
            => new AggregateRootCheckpointResult(aggregateRootId, AggregateRootCheckpointLifetime.Stored, AggregateRootCheckpointStatus.Succeed, message, exception);

        public static AggregateRootCheckpointResult StorageFailed(string aggregateRootId, string message = null, Exception exception = null)
            => new AggregateRootCheckpointResult(aggregateRootId, AggregateRootCheckpointLifetime.Storing, AggregateRootCheckpointStatus.Failed, message, exception);

        public static AggregateRootCheckpointResult StorageTimeOuted(string aggregateRootId, string message = null, Exception exception = null)
            => new AggregateRootCheckpointResult(aggregateRootId, AggregateRootCheckpointLifetime.Storing, AggregateRootCheckpointStatus.TimeOuted, message, exception);

        public static AggregateRootCheckpointResult HandleSucceed(string aggregateRootId, string message = null, Exception exception = null)
            => new AggregateRootCheckpointResult(aggregateRootId, AggregateRootCheckpointLifetime.Handled, AggregateRootCheckpointStatus.Succeed, message, exception);
    }

    public class DomainEventResult<TAggregateRootId> : AggregateRootCheckpointResult
    {
        public DomainEventResult(
            TAggregateRootId aggregateRootId,
            AggregateRootCheckpointLifetime lifetime,
            AggregateRootCheckpointStatus status,
            string message = null,
            Exception exception = null)
            : base(aggregateRootId.ToString(), lifetime, status, message, exception)
        {
            AggregateRootId = aggregateRootId;
        }

        public new TAggregateRootId AggregateRootId { get; }
    }

    public static class DomainEventResultExtensions
    {
        public static bool StorageSucceed(this AggregateRootCheckpointResult result)
            => result.Lifetime == AggregateRootCheckpointLifetime.Stored &&
               result.Status == AggregateRootCheckpointStatus.Succeed;

        public static bool StorageFailed(this AggregateRootCheckpointResult result)
            => result.Lifetime == AggregateRootCheckpointLifetime.Storing &&
               result.Status == AggregateRootCheckpointStatus.Failed;

        public static bool StorageTimeOuted(this AggregateRootCheckpointResult result)
            => result.Lifetime == AggregateRootCheckpointLifetime.Storing &&
               result.Status == AggregateRootCheckpointStatus.TimeOuted;

        public static bool HandleSucceed(this AggregateRootCheckpointResult result)
            => result.Lifetime == AggregateRootCheckpointLifetime.Handled &&
               result.Status == AggregateRootCheckpointStatus.Succeed;
    }
}
