using System;
using System.Buffers.Text;
using System.IO;
using System.Threading.Tasks;

namespace OrleansClient
{
    public class MemoryDemo
    {
        public static async Task<int> ChecksumReadAsync(Memory<byte> buffer, Stream stream)
        {
            int bytesRead = await stream.ReadAsync(buffer);
            return Checksum(buffer.Span.Slice(0, bytesRead));
        }

        static int Checksum(Span<byte> buffer)
        {
            return BitConverter.ToInt32(buffer);
        }

        public static void Split()
        {
            string input = "3123,3123";
            int commaPos = input.IndexOf(',');
            int first = int.Parse(input.Substring(0, commaPos));
            int second = int.Parse(input.Substring(commaPos + 1));

            Console.WriteLine(first);
            Console.WriteLine(second);
        }

        public static void SplitSpan()
        {
            string input = "3123,3123";
            var inputSpan = input.AsSpan();
            int commaPos = input.IndexOf(',');
            int first = int.Parse(inputSpan.Slice(0, commaPos));
            int second = int.Parse(inputSpan.Slice(commaPos + 1));

            Console.WriteLine(first);
            Console.WriteLine(second);
        }

        public static void GenerateId()
        {
            var length = 10;
            var rand = new Random();
            var chars = new char[length];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(rand.Next(0, 10) + '0');
            }

            var id = new string(chars);

            Console.WriteLine(id);
        }

        public static void GenerateIdSpan()
        {
            var length = 10;
            var rand = new Random();
            Span<char> chars = stackalloc char[length];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(rand.Next(0, 10) + '0');
            }
            var id = new string(chars);

            Console.WriteLine(id);
        }

        public static void GenerateIdSpan1()
        {
            var length = 10;
            var rand = new Random();
            var id = String.Create(length, rand, (Span<char> chars, Random r) =>
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = (char)(r.Next(0, 10) + '0');
                }
            });

            Console.WriteLine(id);
        }

        public static void Test2()
        {
            var buffer = new byte[10];
            var utf8Text = new ReadOnlySpan<byte>(buffer);
            if (!Utf8Parser.TryParse(utf8Text, out Guid value, out int bytesConsumed, 'P'))
            {
                throw new InvalidDataException();
            }
        }
    }
}
