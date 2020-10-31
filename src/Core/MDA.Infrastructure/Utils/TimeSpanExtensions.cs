using System;

namespace MDA.Infrastructure.Utils
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Multiply(this TimeSpan timeSpan, double value)
        {
            var ticksD = checked((double)timeSpan.Ticks * value);
            var ticks = checked((long)ticksD);

            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan Divide(this TimeSpan timeSpan, double value)
        {
            var ticksD = checked((double)timeSpan.Ticks / value);
            var ticks = checked((long)ticksD);

            return TimeSpan.FromTicks(ticks);
        }

        public static double Divide(this TimeSpan first, TimeSpan second)
        {
            var ticks1 = (double)first.Ticks;
            var ticks2 = (double)second.Ticks;

            return ticks1 / ticks2;
        }

        public static TimeSpan Max(TimeSpan first, TimeSpan second)
        {
            return first >= second ? first : second;
        }

        public static TimeSpan Min(TimeSpan first, TimeSpan second)
        {
            return first < second ? first : second;
        }

        public static TimeSpan NextTimeSpan(this SafeRandom random, TimeSpan timeSpan)
        {
            if (timeSpan <= TimeSpan.Zero) 
                throw new ArgumentOutOfRangeException(nameof(timeSpan), timeSpan, "SafeRandom.NextTimeSpan timeSpan must be a positive number.");

            var ticksDouble = timeSpan.Ticks * random.NextDouble();
            var ticks = checked((long)ticksDouble);

            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan NextTimeSpan(this SafeRandom random, TimeSpan minValue, TimeSpan maxValue)
        {
            if (minValue <= TimeSpan.Zero) 
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue, "SafeRandom.NextTimeSpan minValue must be a positive number.");
            if (minValue >= maxValue) 
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue, "SafeRandom.NextTimeSpan minValue must be less than maxValue.");

            var span = maxValue - minValue;

            return minValue + random.NextTimeSpan(span);
        }
    }
}
