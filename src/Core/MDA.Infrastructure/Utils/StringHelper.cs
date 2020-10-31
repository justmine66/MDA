using System;

namespace MDA.Infrastructure.Utils
{
    public static class StringHelper
    {
        public static string Substring(this string str, int start, int length)
        {
            return str.AsSpan(start, length).ToString();
        }

        public static string Substring(this string str, int start)
        {
            return str.AsSpan(start, str.Length - start).ToString();
        }

        public static int ParseToInt(this string str, int start, int count)
        {
            return int.Parse(Substring(str, start, count));
        }
    }
}
