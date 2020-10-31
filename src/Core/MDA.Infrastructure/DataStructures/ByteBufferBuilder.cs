using MDA.Infrastructure.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Infrastructure.DataStructures
{
    public class ByteBufferBuilder : IEnumerable<byte[]>
    {
        private static readonly List<byte[]> Buffer = new List<byte[]>();

        public IEnumerator<byte[]> GetEnumerator() => Buffer.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Append(byte[] item) => Buffer.Add(item);

        public void Clear() => Buffer.Clear();

        public int Size => Buffer.Sum(it => it.Length);

        public byte[] Build()
        {
            var buffer = Size <= 1024
                ? stackalloc byte[Size]
                : new byte[Size];

            var offset = 0;

            foreach (var bytes in Buffer)
            {
                foreach (var b in bytes)
                {
                    buffer[offset] = b;
                    offset++;
                }
            }

            return buffer.ToArray();
        }
    }

    public static class ByteBufferBuilderExtensions
    {
        public static ByteBufferBuilder AppendString(this ByteBufferBuilder builder, string value)
        {
            builder.Append(BinarySerializationHelper.SerializeString(value));

            return builder;
        }

        public static ByteBufferBuilder AppendDatetime(this ByteBufferBuilder builder, DateTime value)
        {

            builder.Append(BinarySerializationHelper.SerializeDateTime(value).ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendInt(this ByteBufferBuilder builder, int value)
        {

            builder.Append(BinarySerializationHelper.SerializeInt(value).ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendLong(this ByteBufferBuilder builder, long value)
        {

            builder.Append(BinarySerializationHelper.SerializeLong(value).ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendShort(this ByteBufferBuilder builder, short value)
        {

            builder.Append(BinarySerializationHelper.SerializeShort(value).ToArray());

            return builder;
        }
    }
}
