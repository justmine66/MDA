using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MDA.Infrastructure.Utils
{
    public static class BinarySerializationHelper
    {
        public const int IntSize = sizeof(int);
        public const int LongSize = sizeof(long);
        public const int ShortSize = sizeof(short);

        public const int DatetimeSize = LongSize;
        public const int StringSize = IntSize;

        #region [ Strings ]

        public static byte[] SerializeString(string source)
            => TrySerializeString(source, out var lengthBytes, out var stringBytes)
                ? Union(lengthBytes, stringBytes)
                : Span<byte>.Empty.ToArray();
        public static bool TrySerializeString(
            string source,
            out Span<byte> lengthBytes,
            out Span<byte> stringBytes)
        {
            if (source != null)
            {
                stringBytes = Encoding.UTF8.GetBytes(source);
                lengthBytes = BitConverter.GetBytes(stringBytes.Length);
            }
            else
            {
                stringBytes = Span<byte>.Empty;
                lengthBytes = Span<byte>.Empty;
            }

            return true;
        }

        public static string DeserializeString(
            Span<byte> source,
            int offset = 0)
            => DeserializeString(source, offset, out var nextOffset);
        public static string DeserializeString(
            Span<byte> source,
            int offset,
            out int nextOffset)
            => TryDeserializeString(source, offset, out nextOffset, out var stringBytes)
                ? Encoding.UTF8.GetString(stringBytes)
                : string.Empty;
        public static bool TryDeserializeString(
            Span<byte> source,
            int offset,
            out int nextOffset,
            out byte[] stringBytes)
        {
            if (TryDecodeBytesOnLength(source, offset, StringSize, out var valueBytes, out nextOffset))
            {
                stringBytes = valueBytes.ToArray();

                return true;
            }

            stringBytes = Span<byte>.Empty.ToArray();

            return false;
        }

        #endregion

        public static Span<byte> SerializeDateTime(DateTime source)
            => BitConverter.GetBytes(source.Ticks);
        public static DateTime DeserializeDateTime(Span<byte> source, int offset, out int nextOffset)
        {
            var bytes = DecodeBytes(source, offset, DatetimeSize, out nextOffset);

            return new DateTime(BitConverter.ToInt64(bytes.ToArray(), 0));
        }

        public static Span<byte> SerializeShort(short source)
            => BitConverter.GetBytes(source);
        public static short DeserializeShort(Span<byte> source, int offset, out int nextOffset)
        {
            var bytes = DecodeBytes(source, offset, ShortSize, out nextOffset);

            return BitConverter.ToInt16(bytes.ToArray(), 0);
        }

        public static Span<byte> SerializeInt(int source)
            => BitConverter.GetBytes(source);
        public static int DeserializeInt(Span<byte> source, int offset, out int nextOffset)
        {
            var bytes = DecodeBytes(source, offset, IntSize, out nextOffset);

            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }

        public static Span<byte> SerializeLong(long source)
            => BitConverter.GetBytes(source);
        public static long DeserializeLong(Span<byte> source, int offset, out int nextOffset)
        {
            var bytes = DecodeBytes(source, offset, LongSize, out nextOffset);

            return BitConverter.ToInt64(bytes.ToArray(), 0);
        }

        public static bool TryDecodeBytesOnLength(
            Span<byte> source,
            int offset,
            int lengthSize,
            out Span<byte> valueBytes,
            out int nextOffset)
        {
            var lengthBytes = source.Slice(offset, lengthSize);

            if (MemoryMarshal.TryRead<int>(lengthBytes, out var length))
            {
                offset += lengthSize;

                valueBytes = source.Slice(offset, length);

                offset += length;
                nextOffset = offset;

                return true;
            }

            valueBytes = Span<byte>.Empty;
            nextOffset = offset;

            return false;
        }

        public static Span<byte> DecodeBytes(
            Span<byte> source,
            int offset,
            int count,
            out int nextOffset)
        {
            nextOffset = offset + count;

            return source.Slice(offset, count); ;
        }

        public static byte[] Union(Span<byte> first, Span<byte> second)
        {
            var length1 = first.Length;
            var length2 = second.Length;
            var length = length1 + second.Length;
            var buffer = length < 1024 ? stackalloc byte[length] : new byte[length];

            for (var i = 0; i < length1; i++)
            {
                buffer[i] = first[i];
            }

            for (var i = 0; i < length2; i++)
            {
                buffer[i + length1] = second[i];
            }

            return buffer.ToArray();
        }
    }
}
