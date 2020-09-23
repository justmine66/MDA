using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootStateBackend
    {
        Task<IEventSourcedAggregateRoot> GetAsync(string aggregateRootId, CancellationToken token = default);
    }
}
