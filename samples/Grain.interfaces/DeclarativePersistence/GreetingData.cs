using Orleans.Concurrency;
using System;

namespace Grain.interfaces.DeclarativePersistence
{
    [Immutable]
    public class GreetingData
    {
        public Guid From { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
    }
}
