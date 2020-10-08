using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace MDA.Shared.Utils
{
    /// <summary>
    /// Thread-safe random number generator.
    /// Has same API as System.Random but is thread safe, similar to the implementation by Steven Toub: http://blogs.msdn.com/b/pfxteam/archive/2014/10/20/9434171.aspx
    /// </summary>
    public class SafeRandom
    {
        private static readonly RandomNumberGenerator GlobalCryptoProvider = RandomNumberGenerator.Create();

        [ThreadStatic]
        private static Random _random;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Random GetRandom()
        {
            if (_random != null) return _random;

            var buffer = new byte[4];

            GlobalCryptoProvider.GetBytes(buffer);
            _random = new Random(BitConverter.ToInt32(buffer, 0));

            return _random;
        }

        public int Next() => GetRandom().Next();

        public int Next(int maxValue) => GetRandom().Next(maxValue);

        public int Next(int minValue, int maxValue) => GetRandom().Next(minValue, maxValue);

        public void NextBytes(byte[] buffer) => GetRandom().NextBytes(buffer);

        public double NextDouble() => GetRandom().NextDouble();
    }
}
