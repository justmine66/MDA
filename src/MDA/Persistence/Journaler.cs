using MDA.Eventing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Persistence
{
    public class Journaler : IJournalable
    {
        public Task<bool> LogToStorage(InboundEvent @event)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogToStorage(IEnumerable<InboundEvent> events)
        {
            throw new NotImplementedException();
        }
    }
}
