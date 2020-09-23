using MDA.Shared.Hashes;
using System;

namespace MDA.Shared.DataStructures
{
    public class BloomFilter
    {
        /*
		    Bloom filter implementation based on the following paper by Adam Kirsch and Michael Mitzenmacher:
		    "Less Hashing, Same Performance: Building a Better Bloom Filter"
		    https://www.eecs.harvard.edu/~michaelm/postscripts/rsa2008.pdf
		    Only two 32-bit hash functions can be used to simulate additional hash functions of the form g(x) = h1(x) + i*h2(x)
		*/
        readonly int _m; //number of bits
        readonly int _k; //number of hash functions
        readonly ulong[] _bits; //bit array
        const int LongSize = sizeof(long); //64 bits

        public int NumBits => _m;
        public int NumHashFunctions => _k;

        private static readonly IHasher Hasher1 = new XXHashUnsafe(), Hasher2 = new Murmur3AUnsafe();

        public BloomFilter(int n, double p)
        {
            if (p <= 0.0 || p >= 0.5)
                throw new ArgumentOutOfRangeException("p", "p should be between 0 and 0.5 exclusive");

            //calculate number of hash functions to use
            _k = (int)Math.Ceiling(-Math.Log(p) / Math.Log(2));
            _k = Math.Max(2, _k);

            //calculate number of bits required
            var m = (long)Math.Ceiling(-n * Math.Log(p) / Math.Log(2) / Math.Log(2));
            var buckets = m / LongSize;
            if (m % LongSize != 0) buckets++;
            if (m > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("p",
                    "calculated number of bits, m, is too large: " + m + ". please choose a larger value of p.");
            }

            this._m = (int)m;
            _bits = new ulong[buckets];
        }

        public void Add(long item)
        {
            var bytes = ToBytes(item);
            var hash1 = (int)Hasher1.Hash(bytes);
            var hash2 = (int)Hasher2.Hash(bytes);

            var hash = hash1;
            for (var i = 0; i < _k; i++)
            {
                hash += hash2;
                hash &= int.MaxValue; //make positive
                var idx = (hash % _m);
                _bits[idx / LongSize] |= (1UL << (idx % LongSize));
            }
        }

        public bool MayExist(long item)
        {
            var bytes = ToBytes(item);
            var hash1 = (int)Hasher1.Hash(bytes);
            var hash2 = (int)Hasher2.Hash(bytes);

            var hash = hash1;
            for (var i = 0; i < _k; i++)
            {
                hash += hash2;
                hash &= int.MaxValue; //make positive
                var idx = (hash % _m);
                if ((_bits[idx / LongSize] & (1UL << (idx % LongSize))) == 0)
                    return false;
            }

            return true;
        }

        public static byte[] ToBytes(long value)
        {
            var bytes = new byte[8];

            for (var i = 0; i < 8; i++)
            {
                bytes[i] |= (byte)(value & 0xFF);
                value >>= 8;
            }

            return bytes;
        }
    }
}
