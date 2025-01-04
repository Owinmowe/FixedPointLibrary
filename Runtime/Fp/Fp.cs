using System;
using System.Runtime.CompilerServices;

namespace FixedPoint
{
    [System.Serializable]
    public partial struct Fp : IEquatable<Fp>, IComparable<Fp>, IFormattable
    {
        public const long InternalMaxValue = long.MaxValue;
        public static readonly Fp MaxValue = new(InternalMaxValue);
        
        public const long InternalMinValue = long.MinValue;
        public static readonly Fp MinValue = new(InternalMinValue);
        
        public const long InternalOne = 1L << FractionalPlaces;
        public static readonly Fp One = new(InternalOne);
        public static readonly Fp Zero = new(0);
        
        public const int BitsNumbers = 64;
        public const int FractionalPlaces = 32;

        public long RawValue;

        public Fp(long rawValue)
        {
            RawValue = rawValue;
        }

        public Fp(int value)
        {
            RawValue = value * InternalOne;
        }

        public Fp(float value)
        {
            RawValue = (long)(value * InternalOne);
        }
        
        /// <summary>
        /// Adds x and y. Performs saturating addition, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static Fp operator +(Fp x, Fp y)
        {
            var xl = x.RawValue;
            var yl = y.RawValue;
            var sum = xl + yl;
            
            if (((~(xl ^ yl) & (xl ^ sum)) & InternalMinValue) != 0)
            {
                sum = xl > 0 ? InternalMaxValue : InternalMinValue;
            }
            return new Fp(sum);
        }
        
        /// <summary>
        /// Subtracts y from x. Performs saturating substraction, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static Fp operator -(Fp x, Fp y)
        {
            var xl = x.RawValue;
            var yl = y.RawValue;
            var diff = xl - yl;
            
            if ((((xl ^ yl) & (xl ^ diff)) & InternalMinValue) != 0)
            {
                diff = xl < 0 ? InternalMinValue : InternalMaxValue;
            }
            return new Fp(diff);
        }

        /// <summary>
        /// Divides y with x. Tries to take advantage if the divider is divisible by 2^n by doing bit division.
        /// </summary>
        public static Fp operator /(Fp x, Fp y)
        {
            var xl = x.RawValue;
            var yl = y.RawValue;

            if (yl == 0)
            {
                throw new DivideByZeroException();
            }

            var remainder = (ulong)(xl >= 0 ? xl : -xl);
            var divider = (ulong)(yl >= 0 ? yl : -yl);
            var quotient = 0UL;
            var bitPos = BitsNumbers / 2 + 1;


            // If the divider is divisible by 2^n, take advantage of it.
            while ((divider & 0xF) == 0 && bitPos >= 4)
            {
                divider >>= 4;
                bitPos -= 4;
            }

            while (remainder != 0 && bitPos >= 0)
            {
                int shift = CountLeadingZeroes(remainder);
                if (shift > bitPos)
                {
                    shift = bitPos;
                }
                remainder <<= shift;
                bitPos -= shift;

                var div = remainder / divider;
                remainder = remainder % divider;
                quotient += div << bitPos;

                // Detect overflow
                if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0)
                {
                    return ((xl ^ yl) & InternalMinValue) == 0 ? MaxValue : MinValue;
                }

                remainder <<= 1;
                --bitPos;
            }

            // rounding
            ++quotient;
            var result = (long)(quotient >> 1);
            if (((xl ^ yl) & InternalMinValue) != 0)
            {
                result = -result;
            }

            return new Fp(result);
        } 
        
        /// <summary>
        /// Divides y with x. checks for operation overflow.
        /// </summary>
        public static Fp operator *(Fp x, Fp y)
        {

            var xl = x.RawValue;
            var yl = y.RawValue;

            var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
            var xhi = xl >> FractionalPlaces;
            var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
            var yhi = yl >> FractionalPlaces;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            var loResult = lolo >> FractionalPlaces;
            var midResult1 = lohi;
            var midResult2 = hilo;
            var hiResult = hihi << FractionalPlaces;

            bool overflow = false;
            var sum = AddOverflowHelper((long)loResult, midResult1, ref overflow);
            sum = AddOverflowHelper(sum, midResult2, ref overflow);
            sum = AddOverflowHelper(sum, hiResult, ref overflow);

            bool opSignsEqual = ((xl ^ yl) & InternalMinValue) == 0;

            // if signs of operands are equal and sign of result is negative,
            // then multiplication overflowed positively
            // the reverse is also true
            if (opSignsEqual)
            {
                if (sum < 0 || (overflow && xl > 0))
                {
                    return MaxValue;
                }
            }
            else
            {
                if (sum > 0)
                {
                    return MinValue;
                }
            }

            // if the top 32 bits of hihi (unused in the result) are neither all 0s or 1s,
            // then this means the result overflowed.
            var topCarry = hihi >> FractionalPlaces;
            if (topCarry != 0 && topCarry != -1 /*&& xl != -17 && yl != -17*/)
            {
                return opSignsEqual ? MaxValue : MinValue;
            }

            // If signs differ, both operands' magnitudes are greater than 1,
            // and the result is greater than the negative operand, then there was negative overflow.
            if (!opSignsEqual)
            {
                long posOp, negOp;
                if (xl > yl)
                {
                    posOp = xl;
                    negOp = yl;
                }
                else
                {
                    posOp = yl;
                    negOp = xl;
                }
                if (sum > negOp && negOp < -InternalOne && posOp > InternalOne)
                {
                    return MinValue;
                }
            }

            return new Fp(sum);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fp operator %(Fp x, Fp y)
        {
            return new Fp(
                x.RawValue == InternalMinValue & y.RawValue == -1 ?
                0 :
                x.RawValue % y.RawValue);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fp x, Fp y)
        {
            return x.RawValue == y.RawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fp x, Fp y)
        {
            return x.RawValue != y.RawValue;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fp x, Fp y)
        {
            return x.RawValue > y.RawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fp x, Fp y)
        {
            return x.RawValue < y.RawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fp x, Fp y)
        {
            return x.RawValue >= y.RawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fp x, Fp y)
        {
            return x.RawValue <= y.RawValue;
        }

        public int CompareTo(Fp other)
        {
            return RawValue.CompareTo(other.RawValue);
        }

        public bool Equals(Fp other)
        {
            return RawValue == other.RawValue;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Fp other && Equals(other);
        }

        public override string ToString()
        {
            // Up to 10 decimal places
            return ((decimal)this).ToString("0.##########");
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // Up to 10 decimal places
            return ((decimal)this).ToString("0.##########", formatProvider);
        }
        
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }
    }
}
