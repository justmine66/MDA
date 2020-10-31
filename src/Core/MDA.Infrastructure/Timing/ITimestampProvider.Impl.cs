using System;

namespace MDA.Infrastructure.Timing
{
    public class TimestampProvider : ITimestampProvider
    {
        public long GetTimestamp() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
