using MDA.Eventing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Cluster
{
    public class Replicator : IReplicatable
    {
        public Task<bool> ToSlave(InboundEvent @event)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ToSlave(IEnumerable<InboundEvent> events)
        {
            throw new NotImplementedException();
        }
    }
}
