using System;

namespace MDA.Infrastructure.DataStructures
{
    /// <summary>
    /// 时间戳单位
    /// </summary>
    [Flags]
    public enum TimestampUnit
    {
        /// <summary>
        /// 秒
        /// </summary>
        Second,

        /// <summary>
        /// 毫秒
        /// </summary>
        Millisecond
    }

    public struct Timestamp : IEquatable<Timestamp>
    {
        public long UnixTimestamp { get; }

        public TimestampUnit Unit { get; }

        public DateTime DateTime => Unit == TimestampUnit.Second
            ? DateTimeOffset.FromUnixTimeSeconds(UnixTimestamp).UtcDateTime
            : DateTimeOffset.FromUnixTimeMilliseconds(UnixTimestamp).UtcDateTime;

        public DateTimeOffset DateTimeOffset => Unit == TimestampUnit.Second
            ? DateTimeOffset.FromUnixTimeSeconds(UnixTimestamp)
            : DateTimeOffset.FromUnixTimeMilliseconds(UnixTimestamp);

        public Timestamp(long timestamp, TimestampUnit unit)
        {
            UnixTimestamp = timestamp;
            Unit = unit;
        }

        public Timestamp(DateTimeOffset dateTimeOffset, TimestampUnit unit = TimestampUnit.Millisecond)
            : this(unit == TimestampUnit.Second
                ? dateTimeOffset.ToUnixTimeSeconds()
                : dateTimeOffset.ToUnixTimeMilliseconds(),
                unit)
        { }

        public Timestamp(DateTime dateTime, TimestampUnit unit = TimestampUnit.Millisecond)
            : this(new DateTimeOffset(dateTime), unit)
        { }

        public bool Equals(Timestamp other)
            => other.Unit == Unit && other.UnixTimestamp == UnixTimestamp;

        public override int GetHashCode()
            => Unit.GetHashCode() * 251 + UnixTimestamp.GetHashCode();

        public override bool Equals(object obj)
            => obj is Timestamp other && Equals(other);

        public static bool operator ==(Timestamp left, Timestamp right)
            => left.Equals(right);

        public static bool operator !=(Timestamp left, Timestamp right)
            => !(left == right);

        public static implicit operator DateTimeOffset(Timestamp timestamp)
            => timestamp.DateTimeOffset;

        public static implicit operator DateTime(Timestamp timestamp)
            => timestamp.DateTime;

        public static implicit operator Timestamp(long timestampInMilliseconds)
            => new Timestamp(timestampInMilliseconds, TimestampUnit.Millisecond);

        public static implicit operator long(Timestamp timestamp)
            => timestamp.UnixTimestamp;
    }
}
