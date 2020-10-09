using MDA.Shared.DataStructures;
using System;
using Xunit;

namespace MDA.XUnitTest.Shared
{
    public class TimestampTester
    {
        [Fact]
        public void TestTimestampUtils()
        {
            // 1. millisecond test
            var nowOffset = DateTimeOffset.Now;
            var now = nowOffset.DateTime;
            var baseLine = nowOffset.ToUnixTimeMilliseconds();

            var timestamp1 = new Timestamp(baseLine, TimestampUnit.Millisecond);
            var timestamp2 = new Timestamp(now);
            var timestamp3 = new Timestamp(nowOffset);

            Assert.Equal(timestamp1, timestamp2);
            Assert.Equal(timestamp2, timestamp3);
            Assert.Equal(timestamp1.UnixTimestamp, baseLine);

            Timestamp t1 = baseLine;
            DateTimeOffset dtf1 = t1;
            DateTime dt1 = t1;

            Assert.Equal(t1, dtf1);
            Assert.Equal(t1, dt1);

            // 2. second test
            baseLine = nowOffset.ToUnixTimeSeconds();

            var timestamp4 = new Timestamp(baseLine, TimestampUnit.Second);
            var timestamp5 = new Timestamp(now, TimestampUnit.Second);
            var timestamp6 = new Timestamp(nowOffset, TimestampUnit.Second);

            Assert.NotEqual(timestamp4, timestamp2);
            Assert.Equal(timestamp4, timestamp5);
            Assert.Equal(timestamp5, timestamp6);
            Assert.Equal(timestamp4.UnixTimestamp, baseLine);

            Timestamp t2 = baseLine;
            DateTimeOffset dtf2 = t2;
            DateTime dt2 = t2;

            Assert.Equal(t2, dtf2);
            Assert.Equal(t2, dt2);
        }
    }
}
