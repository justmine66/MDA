using System;
using System.Collections.Generic;

namespace OrleansClient
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitWithSpan(this string input, char separator)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (!input.Contains(separator))
            {
                yield return input;
            }

            int startIndex = 0;
            int separatorIndex = input.IndexOf(separator, startIndex);

            while (separatorIndex > 0)
            {
                var splitSpan = input.AsSpan().Slice(startIndex, separatorIndex - startIndex);
                yield return new string(splitSpan);

                startIndex = separatorIndex + 1;
                separatorIndex = input.IndexOf(separator, startIndex);
                if (separatorIndex == -1)
                {
                    var tailSpan = input.AsSpan().Slice(startIndex, input.Length - startIndex);
                    yield return new string(tailSpan);
                }
            }
        }
    }
}
