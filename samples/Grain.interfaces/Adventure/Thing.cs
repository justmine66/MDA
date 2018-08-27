using Orleans.Concurrency;
using System;
using System.Collections.Generic;

namespace Grain.interfaces.Adventure
{
    [Immutable]
    public class Thing
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public long FoundIn { get; set; }
        public List<String> Commands { get; set; }
    }
}
