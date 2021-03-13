using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    /// <summary>
    /// 领域事件状态后端
    /// </summary>
    public interface IDomainEventStateBackend
    {
        /// <summary>
        /// 追加领域事件
        /// </summary>
        /// <param name="event">领域事件</param>
        /// <param name="token">取消令牌</param>
        /// <returns>追加结果</returns>
        Task<DomainEventResult> AppendAsync(IDomainEvent @event, CancellationToken token = default);

        /// <summary>
        /// 追加领域事件列表
        /// </summary>
        /// <param name="events">领域事件列表</param>
        /// <param name="token">取消令牌</param>
        /// <returns>追加结果</returns>
        Task<IEnumerable<DomainEventResult>> AppendAsync(IEnumerable<IDomainEvent> @events, CancellationToken token = default);

        /// <summary>
        /// 获取聚合根领域事件序列
        /// </summary>
        /// <param name="aggregateRootId">聚合根标识</param>
        /// <param name="startOffset">起始位点</param>
        /// <param name="token">取消令牌</param>
        /// <returns>领域事件序列</returns>
        Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            long startOffset = 0, 
            CancellationToken token = default);

        /// <summary>
        /// 获取聚合根领域事件序列
        /// </summary>
        /// <param name="aggregateRootId">聚合根标识</param>
        /// <param name="startOffset">起始位点</param>
        /// <param name="endOffset">截止位点</param>
        /// <param name="token">取消令牌</param>
        /// <returns>领域事件序列</returns>
        Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(
            string aggregateRootId,
            long startOffset = 0,
            long endOffset = long.MaxValue,
            CancellationToken token = default);

        /// <summary>
        /// 统计指标
        /// </summary>
        /// <param name="aggregateRootId">聚合根标识</param>
        /// <param name="generation">第几代聚合根,随着检查点(Checkpoint)快照而递增.</param>
        /// <param name="token">取消令牌</param>
        /// <returns>指标信息</returns>
        Task<DomainEventMetrics> StatMetricsAsync(
            string aggregateRootId,
            int generation,
            CancellationToken token = default);
    }
}
