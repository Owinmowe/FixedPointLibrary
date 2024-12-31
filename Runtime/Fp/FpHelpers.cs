using System.Runtime.CompilerServices;

namespace FixedPoint
{
    public partial struct Fp
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CountLeadingZeroes(ulong x)
        {
            int result = 0;
            while ((x & 0xF000000000000000) == 0)
            {
                result += 4;
                x <<= 4;
            }

            while ((x & 0x8000000000000000) == 0)
            {
                result += 1;
                x <<= 1;
            }

            return result;
        }
        
        private static long AddOverflowHelper(long x, long y, ref bool overflow)
        {
            var sum = x + y;
            overflow |= ((x ^ y ^ sum) & InternalMinValue) != 0;
            return sum;
        }
    }
}