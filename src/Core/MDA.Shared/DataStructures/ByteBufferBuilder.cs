using MDA.Shared.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Shared.DataStructures
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
            var buffer = Size <= 128
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
            BinaryConverter.EncodeString(value, out var lengthBytes, out var stringBytes);

            builder.Append(lengthBytes.ToArray());
            builder.Append(stringBytes.ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendDatetime(this ByteBufferBuilder builder, DateTime value)
        {

            builder.Append(BinaryConverter.EncodeDateTime(value).ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendInt(this ByteBufferBuilder builder, int value)
        {

            builder.Append(BinaryConverter.EncodeInt(value).ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendLong(this ByteBufferBuilder builder, long value)
        {

            builder.Append(BinaryConverter.EncodeLong(value).ToArray());

            return builder;
        }

        public static ByteBufferBuilder AppendShort(this ByteBufferBuilder builder, short value)
        {

            builder.Append(BinaryConverter.EncodeShort(value).ToArray());

            return builder;
        }
    }
}
