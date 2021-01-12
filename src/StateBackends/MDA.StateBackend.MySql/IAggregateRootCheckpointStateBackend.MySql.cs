using MDA.Domain.Models;
using MDA.Infrastructure.Async;
using MDA.Infrastructure.Serialization;
using MDA.Infrastructure.Typing;
using MDA.Infrastructure.Utils;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.MySql
{
    public class MySqlAggregateRootCheckpointStateBackend<TPayload> : IAggregateRootCheckpointStateBackend<TPayload> where TPayload : ISerializationMetadataProvider
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;
        private readonly IBinarySerializer _binarySerializer;
        private readonly ITypeResolver _typeResolver;
        private readonly MySqlStateBackendOptions _options;

        public MySqlAggregateRootCheckpointStateBackend(
            IRelationalDbStorageFactory storageFactory,
            ILoggerFactory loggerFactory,
            ITypeResolver typeResolver,
            IOptions<MySqlStateBackendOptions> options,
            IBinarySerializer binarySerializer)
        {
            _logger = loggerFactory.CreateLogger(typeof(MySqlAggregateRootCheckpointStateBackend<TPayload>));
            _db = storageFactory.CreateRelationalDbStorage(DatabaseScheme.StateDb);
            _typeResolver = typeResolver;
            _binarySerializer = binarySerializer;
            _options = options.Value;
        }

        public async Task<AggregateRootCheckpointResult> AppendAsync(
            AggregateRootCheckpoint<TPayload> checkpoint,
            CancellationToken token = default)
        {
            PreConditions.NotNull(checkpoint, nameof(checkpoint));
            PreConditions.NotNullOrEmpty(checkpoint.AggregateRootId, nameof(checkpoint.AggregateRootId));
            PreConditions.NotNull(checkpoint.AggregateRootType, nameof(checkpoint.AggregateRootType));
            PreConditions.Nonnegative(checkpoint.AggregateRootGeneration, nameof(checkpoint.AggregateRootGeneration));
            PreConditions.Nonnegative(checkpoint.AggregateRootVersion, nameof(checkpoint.AggregateRootVersion));

            var record = AggregateRootCheckpointRecordPortAdapter.ToRecord(checkpoint, _binarySerializer);
            var parameters = DbParameterProvider.ReflectionParameters(record);

            var tables = _options.Tables.DomainModelOptions;

            var insertCheckpointIndexSql = $"INSERT INTO `{tables.AggregateRootCheckpointIndices}`(`AggregateRootId`,`AggregateRootType`,`AggregateRootVersion`,`AggregateRootGeneration`,`CreatedTimestamp`) VALUES(@AggregateRootId,@AggregateRootType,@AggregateRootVersion,@AggregateRootGeneration,@CreatedTimestamp)";
            var insertCheckpointSql = $"INSERT INTO `{tables.AggregateRootCheckpoints}`(`AggregateRootId`,`Payload`) VALUES (@AggregateRootId,@Payload)";

            var maxNumErrorTries = 3;
            var maxExecutionTime = TimeSpan.FromSeconds(3);
            var expectedRows = 2;
            bool ErrorFilter(Exception exc, int attempt)
            {
                if (exc is MySqlException inner &&
                    inner.HasDuplicateEntry())
                {
                    _logger.LogWarning($"{record.AggregateRootType}: [Ignored]find duplicated aggregate root checkpoint from mysql state backend：{inner.Message}.");

                    return false;
                }

                _logger.LogError($"Append aggregate root checkpoint has unknown exception: {LogFormatter.PrintException(exc)}.");

                return true;
            }

            var affectedRows = await AsyncExecutorWithRetries.ExecuteWithRetriesAsync(async attempt =>
            {
                return await _db.ExecuteAsync(
                    $"{insertCheckpointIndexSql};{insertCheckpointSql};",
                    command => command.Parameters.AddRange(parameters),
                    token);
            }, maxNumErrorTries, ErrorFilter, maxExecutionTime).ConfigureAwait(false);

            if (affectedRows != expectedRows)
                return AggregateRootCheckpointResult.StorageFailed(record.AggregateRootId,
                    $"The affected rows returned MySql state backend is incorrect when append aggregate root checkpoint, expected: {expectedRows}, actual: {affectedRows}.");

            return AggregateRootCheckpointResult.StorageSucceed(record.AggregateRootId);
        }

        public async Task<AggregateRootCheckpoint<TPayload>> GetLatestCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default)
        {
            PreConditions.NotNullOrEmpty(aggregateRootId, nameof(aggregateRootId));
            PreConditions.NotNull(aggregateRootType, nameof(aggregateRootType));
            if (!typeof(IEventSourcedAggregateRoot).IsAssignableFrom(aggregateRootType))
            {
                throw new ArgumentException($"The {aggregateRootType.FullName} cannot assign to {typeof(IEventSourcedAggregateRoot).FullName}.");
            }

            var tables = _options.Tables.DomainModelOptions;

            var sql = $"SELECT d.`AggregateRootId`,d.`AggregateRootType`,d.`AggregateRootVersion`,d.`AggregateRootGeneration`,d.`CreatedTimestamp`, p.`Payload` FROM `{tables.AggregateRootCheckpointIndices}` d INNER JOIN `{tables.AggregateRootCheckpoints}` p ON d.`AggregateRootId`=p.`AggregateRootId` WHERE d.`AggregateRootId`=@AggregateRootId ORDER BY d.`pkId` DESC LIMIT 1";

            var records = await _db.ReadAsync<AggregateRootCheckpointRecord>(sql, new
            {
                AggregateRootId = aggregateRootId,
            }, token);

            return records.IsNotEmpty() 
                ? AggregateRootCheckpointRecordPortAdapter.ToCheckpoint<TPayload>(records.FirstOrDefault(), _typeResolver, _binarySerializer)
                : null;
        }
    }
}
