using System;

namespace OrleansClient
{
    public class ReadOnlySpanDemo
    {
        public static void Test()
        {
            string str = "hello, world";
            string worldString = str.Substring(startIndex: 7, length: 5);
            var worldSpan = str.AsSpan().Slice(start: 7, length: 5);

            Console.WriteLine(worldSpan[0]);
        }
    }
}
