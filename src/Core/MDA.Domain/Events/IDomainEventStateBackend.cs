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
        Task AppendAsync(IDomainEvent @event, CancellationToken token = default);
        Task AppendAsync(IEnumerable<IDomainEvent> @events, CancellationToken token = default);

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
    }
}
