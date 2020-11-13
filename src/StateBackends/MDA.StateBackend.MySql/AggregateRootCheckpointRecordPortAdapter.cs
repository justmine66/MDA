using MDA.Domain.Models;
using MDA.Infrastructure.Serialization;
using MDA.Infrastructure.Typing;
using System;

namespace MDA.StateBackend.MySql
{
    public class AggregateRootCheckpointRecordPortAdapter
    {
        public static AggregateRootCheckpointRecord ToRecord<TPayload>(
            AggregateRootCheckpoint<TPayload> checkpoint,
            IBinarySerializer serializer) where TPayload : ISerializationMetadataProvider
        {
            return new AggregateRootCheckpointRecord()
            {
                AggregateRootId = checkpoint.AggregateRootId,
                AggregateRootType = checkpoint.AggregateRootType.FullName,
                AggregateRootGeneration = checkpoint.AggregateRootGeneration,
                AggregateRootVersion = checkpoint.AggregateRootVersion,
                Payload = serializer.Serialize(checkpoint.Payload, checkpoint.Payload.IgnoreKeys)
            };
        }

        public static AggregateRootCheckpoint<TPayload> ToCheckpoint<TPayload>(
            AggregateRootCheckpointRecord record,
            ITypeResolver typeResolver,
            IBinarySerializer binarySerializer) where TPayload : ISerializationMetadataProvider
        {
            if (!typeResolver.TryResolveType(record.AggregateRootType, out var aggregateRootType))
            {
                throw new TypeLoadException($"Cannot resolve type: {record.AggregateRootType}.");
            }

            if (!(typeof(IEventSourcedAggregateRoot).IsAssignableFrom(aggregateRootType)))
            {
                throw new InvalidOperationException($"The {record.AggregateRootType} cannot assign to {typeof(IEventSourcedAggregateRoot).FullName}.");
            }

            var payload = binarySerializer.Deserialize(record.Payload, aggregateRootType);

            if (payload is TPayload aggregateRoot)
            {
                return new AggregateRootCheckpoint<TPayload>(record.AggregateRootId, aggregateRootType, record.AggregateRootGeneration, record.AggregateRootVersion, aggregateRoot);
            }

            return null;
        }
    }
}
