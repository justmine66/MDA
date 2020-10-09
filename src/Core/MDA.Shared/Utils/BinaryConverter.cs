using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MDA.Shared.Utils
{
    public static class BinaryConverter
    {
        public const int IntSize = sizeof(int);
        public const int LongSize = sizeof(long);
        public const int ShortSize = sizeof(short);

        public const int DatetimeSize = LongSize;
        public const int StringSize = IntSize;

        public static void EncodeString(
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
        }
        public static string DecodeString(Span<byte> source, int startOffset, out int nextStartOffset)
            => TryDecodeBytesOnLength(source, startOffset, StringSize, out var valueBytes, out nextStartOffset)
                ? Encoding.UTF8.GetString(valueBytes.ToArray())
                : string.Empty;

        public static Span<byte> EncodeDateTime(DateTime source)
            => BitConverter.GetBytes(source.Ticks);
        public static DateTime DecodeDateTime(Span<byte> source, int startOffset, out int nextStartOffset)
        {
            var bytes = DecodeBytes(source, startOffset, DatetimeSize, out nextStartOffset);

            return new DateTime(BitConverter.ToInt64(bytes.ToArray(), 0));
        }

        public static Span<byte> EncodeShort(short source)
            => BitConverter.GetBytes(source);
        public static short DecodeShort(Span<byte> source, int startOffset, out int nextStartOffset)
        {
            var bytes = DecodeBytes(source, startOffset, ShortSize, out nextStartOffset);

            return BitConverter.ToInt16(bytes.ToArray(), 0);
        }

        public static Span<byte> EncodeInt(int source)
            => BitConverter.GetBytes(source);
        public static int DecodeInt(Span<byte> source, int startOffset, out int nextStartOffset)
        {
            var bytes = DecodeBytes(source, startOffset, IntSize, out nextStartOffset);

            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }

        public static Span<byte> EncodeLong(long source)
            => BitConverter.GetBytes(source);
        public static long DecodeLong(Span<byte> source, int startOffset, out int nextStartOffset)
        {
            var bytes = DecodeBytes(source, startOffset, LongSize, out nextStartOffset);

            return BitConverter.ToInt64(bytes.ToArray(), 0);
        }

        public static bool TryDecodeBytesOnLength(
            Span<byte> source,
            int startOffset,
            int lengthSize,
            out Span<byte> sink,
            out int nextStartOffset)
        {
            var lengthBytes = source.Slice(startOffset, lengthSize);

            if (MemoryMarshal.TryRead<int>(lengthBytes, out var length))
            {
                startOffset += lengthSize;

                sink = source.Slice(startOffset, length);

                startOffset += length;
                nextStartOffset = startOffset;

                return true;
            }

            sink = Span<byte>.Empty;
            nextStartOffset = startOffset;

            return false;
        }

        public static Span<byte> DecodeBytes(
            Span<byte> source,
            int startOffset,
            int count,
            out int nextStartOffset)
        {
            nextStartOffset = startOffset + count;

            return source.Slice(startOffset, count); ;
        }
    }
}
