using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Infrastructure.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> collection)
        {
            return collection ?? Enumerable.Empty<T>();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            switch (collection)
            {
                case null:
                    return true;
                case ICollection coll:
                    return coll.Count == 0;
                default:
                    return !collection.Any();
            }
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> collection) => !IsEmpty(collection);
    }
}
