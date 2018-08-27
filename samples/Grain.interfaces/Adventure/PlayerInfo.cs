using Orleans.Concurrency;
using System;

namespace Grain.interfaces.Adventure
{
    [Immutable]
    public class PlayerInfo
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
    }
}
