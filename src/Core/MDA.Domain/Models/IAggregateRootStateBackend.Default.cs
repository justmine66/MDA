using MDA.Domain.Events;
using MDA.Infrastructure.Scheduling;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class DefaultAggregateRootStateBackend : IAggregateRootStateBackend
    {
        private readonly ITimer _timer;
        private readonly ILogger _logger;
        private readonly CheckpointTriggerOptions _options;
        private readonly IAggregateRootMemoryCache _memoryCache;
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly IDomainEventStateBackend _eventStateBackend;
        private readonly IAggregateRootCheckpointManager _checkpointManager;

        public DefaultAggregateRootStateBackend(
            ITimer timer,
            IAggregateRootMemoryCache memoryCache,
            IDomainEventStateBackend stateBackend,
            IOptions<CheckpointTriggerOptions> options,
            IAggregateRootFactory aggregateRootFactory,
            ILogger<DefaultAggregateRootStateBackend> logger,
            IAggregateRootCheckpointManager checkpointManager)
        {
            _timer = timer;
            _logger = logger;
            _eventStateBackend = stateBackend;
            _checkpointManager = checkpointManager;
            _memoryCache = memoryCache;
            _aggregateRootFactory = aggregateRootFactory;
            _options = options.Value;
        }

        public async Task<IEventSourcedAggregateRoot> GetAsync<TAggregateRootId>(
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default)
        {
            var aggregateStringId = aggregateRootId?.ToString();
            var defaultAggregateStringId = default(TAggregateRootId)?.ToString();

            if (string.IsNullOrWhiteSpace(aggregateStringId) ||
                aggregateStringId == defaultAggregateStringId)
            {
                throw new ArgumentNullException(nameof(aggregateRootId));
            }

            // 1. 从检查点还原聚合根
            var checkPoint = await _checkpointManager.GetLatestCheckpointAsync(aggregateStringId, aggregateRootType, token);
            var aggregateRoot = checkPoint?.Payload ?? _aggregateRootFactory.CreateAggregateRoot(aggregateRootId, aggregateRootType);

            var aggregateRootFullName = aggregateRootType.FullName;
            _logger.LogInformation($"Got latest checkpoint of the aggregate root, id: {aggregateStringId}, Type: {aggregateRootFullName}, Generation: {aggregateRoot.Generation}, Version: {aggregateRoot.Version}.");

            // 2. 获取从检查点开始产生的事件流
            var eventStream = await _eventStateBackend.GetEventStreamAsync(
                aggregateRoot.Id,
                aggregateRoot.Version,
                token);

            _logger.LogInformation($"Got domain event stream of the aggregate root, id: {aggregateStringId}, Type: {aggregateRootFullName}, Generation: {aggregateRoot.Generation}, Version: {aggregateRoot.Version}.");

            if (eventStream.IsEmpty())
            {
                DoCacheAndCheckpoint(aggregateRoot, token);

                return aggregateRoot;
            }

            // 3.2 重放事件
            try
            {
                aggregateRoot.ReplayDomainEvents(eventStream);
            }
            catch (Exception e)
            {
                _logger.LogError($"Replaying domain event stream for aggregateRoot, Id: {aggregateStringId}, Type: {aggregateRootFullName}, Generation: {aggregateRoot.Generation}, Version: {aggregateRoot.Version} has a unknown exception: {e}.");

                return null;
            }

            _logger.LogInformation($"Replayed domain event stream of the aggregate root, id: {aggregateStringId}, Type: {aggregateRootFullName}, Generation: {aggregateRoot.Generation}, Version: {aggregateRoot.Version}.");

            DoCacheAndCheckpoint(aggregateRoot, token);

            return aggregateRoot;
        }

        public async Task<IEnumerable<DomainEventResult>> AppendMutatingDomainEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken token = default)
            => await _eventStateBackend.AppendAsync(events, token);

        private void DoCacheAndCheckpoint(IEventSourcedAggregateRoot aggregateRoot, CancellationToken token)
        {
            _memoryCache.Set(aggregateRoot);

            _timer.NewTimeout(new FunctionTimerTask(async it => await DoCheckpointAsync(it, aggregateRoot, token)), TimeSpan.FromSeconds(_options.StepInSeconds));
        }

        private async Task DoCheckpointAsync(
            ITimeout timeout,
            IEventSourcedAggregateRoot aggregateRoot,
            CancellationToken token)
        {
            if (timeout.Canceled) return;

            await _checkpointManager.CheckpointAsync(aggregateRoot, token);

            timeout.Timer.NewTimeout(new FunctionTimerTask(async it => await DoCheckpointAsync(it, aggregateRoot, token)), TimeSpan.FromSeconds(_options.StepInSeconds));
        }
    }
}