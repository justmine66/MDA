using System;
using MDA.Shared.DataStructures;
using MDA.Shared.Utils;
using Xunit;

namespace MDA.XUnitTest.Shared
{
    public class BinaryConverterTester
    {
        [Fact]
        public void TestBinaryConverterUtils()
        {
            var str = "1s!A";
            var str1 = "1s!Ab";
            var dt = DateTime.Now;
            var int0 = 1;
            short short0 = 1;
            long long0 = 1;

            var builder = new ByteBufferBuilder()
                .AppendString(str)
                .AppendString(str1)
                .AppendDatetime(dt)
                .AppendInt(int0)
                .AppendShort(short0)
                .AppendLong(long0)
                ;

            var buffer = builder.Build();
            var offset = 0;

            Assert.Equal(BinaryConverter.DecodeString(buffer, offset, out var nextStartOffset), str);
            Assert.Equal(BinaryConverter.DecodeString(buffer, nextStartOffset, out var nextStartOffset1), str1);
            Assert.Equal(BinaryConverter.DecodeDateTime(buffer, nextStartOffset1, out var nextStartOffset2), dt);
            Assert.Equal(BinaryConverter.DecodeInt(buffer, nextStartOffset2, out var nextStartOffset3), int0);
            Assert.Equal(BinaryConverter.DecodeShort(buffer, nextStartOffset3, out var nextStartOffset4), short0);
            Assert.Equal(BinaryConverter.DecodeLong(buffer, nextStartOffset4, out var nextStartOffset5), long0);
        }
    }
}
