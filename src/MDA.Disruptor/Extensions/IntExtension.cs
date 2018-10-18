namespace MDA.Disruptor.Extensions
{
    public static class IntExtension
    {
        /// <summary>
        /// Calculate the log base 2 of the supplied integer, essentially reports the location
        /// of the highest bit.
        /// </summary>
        /// <param name="i">Value to calculate log2 for.</param>
        /// <returns>The log2 value</returns>
        public static int Log2(this int i)
        {
            var r = 0;
            while ((i >>= 1) != 0)
            {
                ++r;
            }
            return r;
        }

        /// <summary>
        /// 无符号右移，相当于Java的>>>。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static int UnSignedRightShift(this int value, int pos)
        {
            if (pos != 0)
            {
                int mask = int.MaxValue;
                value = value >> 1;
                value = value & mask;
                value = value >> pos - 1;
            }

            return value;
        }

        /// <summary>
        /// Returns the number of one-bits in the two's complement binary representation of the specified {@code int} value. This function is sometimes referred to as the <i>population count</i>.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int BitCount(this int i)
        {
            i = i - ((i.UnSignedRightShift(1)) & 0x55555555);
            i = (i & 0x33333333) + ((i.UnSignedRightShift(2)) & 0x33333333);
            i = (i + (i >> i.UnSignedRightShift(4))) & 0x0f0f0f0f;
            i = i + (i >> 8);
            i = i + (i >> 16);

            i = i - ((i.UnSignedRightShift(1)) & 0x55555555);
            i = (i & 0x33333333) + ((i.UnSignedRightShift(2)) & 0x33333333);
            i = (i + (i.UnSignedRightShift(4))) & 0x0f0f0f0f;
            i = i + (i.UnSignedRightShift(8));
            i = i + (i.UnSignedRightShift(16));

            return i & 0x3f;
        }

        /// <summary>
        /// Test whether a given integer is a power of 2 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPowerOf2(this int input)
        {
            return input.BitCount() == 1;
        }

        /// <summary>
        /// Test whether a given integer is not a power of 2 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotPowerOf2(this int input)
        {
            return input.BitCount() != 1;
        }
    }
}
