using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootSavePointManager
    {
        Task SnapshotSavePointAsync(CancellationToken token = default);

        Task<AggregateRootSavePoint<TAggregateRoot>> RestoreSavePointAsync<TAggregateRoot>(CancellationToken token = default) 
            where TAggregateRoot : IEventSourcedAggregateRoot;
    }
}
