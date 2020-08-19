using System.Collections.Generic;

namespace MDA.Domain.Shared
{
    public static class HashHelper
    {
        public static int ComputeHashCode(IEnumerable<object> objects)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objects)
                    hash = hash * 23 + (obj?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public static int ComputeHashCode(params object[] objects)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objects)
                    hash = hash * 23 + (obj?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
