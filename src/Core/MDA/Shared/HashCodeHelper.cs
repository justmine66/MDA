using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MDA.Shared
{
    public class HashCodeHelper
    {
        public static int CombineHashCodes(IEnumerable<object> objs)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objs)
                    hash = hash * 23 + (obj?.GetHashCode() ?? 0);
                return hash;
            }
        }
        public static int GetShardIndexOf(string key, int shardNumber)
        {
            var buffer = Encoding.UTF8.GetBytes(key.ToCharArray());
            var hash = HashAlgorithm.Create().ComputeHash(buffer);

            return BitConverter.ToInt32(hash, 0) % shardNumber;
        }
    }
}
