using MDA.Infrastructure.Hashes;
using System;

namespace MDA.Infrastructure.DataStructures
{
    public class BloomFilter
    {
        private static readonly IHasher Hasher1 = new XXHashUnsafe(), Hasher2 = new Murmur3AUnsafe();

        private const int LongSize = sizeof(long); //64 bits

        /*
		    Bloom filter implementation based on the following paper by Adam Kirsch and Michael Mitzenmacher:
		    "Less Hashing, Same Performance: Building a Better Bloom Filter"
		    https://www.eecs.harvard.edu/~michaelm/postscripts/rsa2008.pdf
		    Only two 32-bit hash functions can be used to simulate additional hash functions of the form g(x) = h1(x) + i*h2(x)
		*/
        private readonly int _numBits; //number of bits
        private readonly int _numHashFunctions; //number of hash functions
        private readonly ulong[] _bits; //bit array

        /// <summary>
        /// number of bits
        /// </summary>
        public int NumBits => _numBits;

        /// <summary>
        /// number of hash functions
        /// </summary>
        public int NumHashFunctions => _numHashFunctions;

        public BloomFilter(int n, double p)
        {
            if (p <= 0.0 || p >= 0.5)
                throw new ArgumentOutOfRangeException(nameof(p), "p should be between 0 and 0.5 exclusive");

            //calculate number of hash functions to use
            _numHashFunctions = (int)Math.Ceiling(-Math.Log(p) / Math.Log(2));
            _numHashFunctions = Math.Max(2, _numHashFunctions);

            //calculate number of bits required
            var m = (long)Math.Ceiling(-n * Math.Log(p) / Math.Log(2) / Math.Log(2));
            var buckets = m / LongSize;
            if (m % LongSize != 0) buckets++;
            if (m > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(p), "calculated number of bits, m, is too large: " + m + ". please choose a larger value of p.");
            }

            _numBits = (int)m;
            _bits = new ulong[buckets];
        }

        public void Add(long item)
        {
            var bytes = ToBytes(item);
            var hash1 = (int)Hasher1.Hash(bytes);
            var hash2 = (int)Hasher2.Hash(bytes);

            var hash = hash1;
            for (var i = 0; i < _numHashFunctions; i++)
            {
                hash += hash2;
                hash &= int.MaxValue; //make positive
                var idx = (hash % _numBits);
                _bits[idx / LongSize] |= (1UL << (idx % LongSize));
            }
        }

        public bool MayExist(long item)
        {
            var bytes = ToBytes(item);
            var hash1 = (int)Hasher1.Hash(bytes);
            var hash2 = (int)Hasher2.Hash(bytes);

            var hash = hash1;
            for (var i = 0; i < _numHashFunctions; i++)
            {
                hash += hash2;
                hash &= int.MaxValue; //make positive
                var idx = (hash % _numBits);
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
