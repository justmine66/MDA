using MDA.Eventing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Persistence
{
    public interface IJournalable
    {
        Task<bool> LogToStorage(InboundEvent @event);
        Task<bool> LogToStorage(IEnumerable<InboundEvent> events);
    }
}
