using System;
using System.Threading;
using System.Threading.Tasks;
using MDA.Infrastructure.Serialization;

namespace MDA.Domain.Models
{
    /// <summary>
    /// 聚合根检查点状态后端
    /// </summary>
    public interface IAggregateRootCheckpointStateBackend<TPayload> 
        where TPayload : ISerializationMetadataProvider
    {
        /// <summary>
        /// 追加聚合根检查点
        /// </summary>
        /// <typeparam name="TPayload">有效载荷</typeparam>
        /// <param name="checkpoint">检查点</param>
        /// <param name="token">取消令牌</param>
        /// <returns>聚合根检查点结果</returns>
        Task<AggregateRootCheckpointResult> AppendAsync(AggregateRootCheckpoint<TPayload> checkpoint,
            CancellationToken token = default);

        /// <summary>
        /// 获取聚合根最新的检查点
        /// </summary>
        /// <param name="aggregateRootId">聚合根标识</param>
        /// <param name="aggregateRootType">聚合根类型</param>
        /// <param name="token">取消令牌</param>
        /// <returns>最新的检查点</returns>
        Task<AggregateRootCheckpoint<TPayload>> GetLatestCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);
    }
}
