using System;

namespace MDA.Shared.Timing
{
    public class TimestampProvider : ITimestampProvider
    {
        public long GetTimestamp() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
