using System;

namespace FixedPoint
{
    public partial class MathFp
    {
        private const long InternalLn2 = 0xB17217F7;
        public static readonly Fp Ln2 = new Fp(InternalLn2);
        
        private const long InternalLog2Max = 0x1F00000000;
        public static readonly Fp Log2Max = new Fp(InternalLog2Max);
        
        private const long InternalLog2Min = -0x2000000000;
        public static readonly Fp Log2Min = new Fp(InternalLog2Min);
        
        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        public static int Sign(Fp value)
        {
            return value.RawValue < 0 ? -1 : value.RawValue > 0 ? 1 : 0;
        }
        
        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
        /// </summary>
        public static Fp Abs(Fp value)
        {
            if (value.RawValue == Fp.InternalMinValue)
            {
                return Fp.MaxValue;
            }

            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.RawValue >> 63;
            return new Fp((value.RawValue + mask) ^ mask);
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        public static Fp Floor(Fp value)
        {
            // Just zero out the fractional part
            ulong floorRawValue = (ulong)value.RawValue & 0xFFFFFFFF00000000;
            return new Fp((long)floorRawValue);
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        public static Fp Ceiling(Fp value)
        {
            var hasFractionalPart = (value.RawValue & 0x00000000FFFFFFFF) != 0;
            return hasFractionalPart ? Floor(value) + Fp.One : value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        public static Fp Round(Fp value)
        {
            var fractionalPart = value.RawValue & 0x00000000FFFFFFFF;
            Fp integerPart = Floor(value);
            if (fractionalPart < 0x80000000)
            {
                return integerPart;
            }
            if (fractionalPart > 0x80000000)
            {
                return integerPart + Fp.One;
            }
            // if number is halfway between two values, round to the nearest even number
            // this is the method used by System.Math.Round().
            return integerPart % 2 == 0
                       ? integerPart
                       : integerPart + Fp.One;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value towards zero.
        /// </summary>
        public static Fp Truncate(Fp value)
        {
            var sign = Sign(value);
            var integralPart = Floor(value);

            if (sign < 0)
            {
                return integralPart + Fp.One;
            }
            else
            {
                return integralPart;
            }
        }
        
        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was negative.
        /// </exception>
        public static Fp Sqrt(Fp x)
        {
            var xl = x.RawValue;
            if (xl < 0)
            {
                // We cannot represent infinities like Single and Double, and Sqrt is
                // mathematically undefined for x < 0. So we just throw an exception.
                throw new ArgumentOutOfRangeException("Negative value passed to Sqrt", "x");
            }

            var num = (ulong)xl;
            var result = 0UL;

            // second-to-top bit
            var bit = 1UL << (Fp.BitsNumbers - 2);

            while (bit > num)
            {
                bit >>= 2;
            }

            // The main part is executed twice, in order to avoid
            // using 128 bit values in computations.
            for (var i = 0; i < 2; ++i)
            {
                // First we get the top 48 bits of the answer.
                while (bit != 0)
                {
                    if (num >= result + bit)
                    {
                        num -= result + bit;
                        result = (result >> 1) + bit;
                    }
                    else
                    {
                        result = result >> 1;
                    }
                    bit >>= 2;
                }

                if (i == 0)
                {
                    // Then process it again to get the lowest 16 bits.
                    if (num > (1UL << (Fp.BitsNumbers / 2)) - 1)
                    {
                        // The remainder 'num' is too large to be shifted left
                        // by 32, so we have to add 1 to result manually and
                        // adjust 'num' accordingly.
                        // num = a - (result + 0.5)^2
                        //       = num + result^2 - (result + 0.5)^2
                        //       = num - result - 0.5
                        num -= result;
                        num = (num << (Fp.BitsNumbers / 2)) - 0x80000000UL;
                        result = (result << (Fp.BitsNumbers / 2)) + 0x80000000UL;
                    }
                    else
                    {
                        num <<= (Fp.BitsNumbers / 2);
                        result <<= (Fp.BitsNumbers / 2);
                    }

                    bit = 1UL << (Fp.BitsNumbers / 2 - 2);
                }
            }
            // Finally, if next bit would have been 1, round the result upwards.
            if (num > result)
            {
                ++result;
            }
            return new Fp((long)result);
        }
        
        /// <summary>
        /// Returns 2 raised to the specified power.
        /// Provides at least 6 decimals of accuracy.
        /// </summary>
        public static Fp Pow2(Fp x)
        {
            if (x.RawValue == 0)
            {
                return Fp.One;
            }

            // Avoid negative arguments by exploiting that exp(-x) = 1/exp(x).
            bool neg = x.RawValue < 0;
            if (neg)
            {
                x = -x;
            }

            if (x == Fp.One)
            {
                return neg ? Fp.One / (Fp)2 : (Fp)2;
            }
            if (x >= Log2Max)
            {
                return neg ? Fp.One / Fp.MaxValue : Fp.MaxValue;
            }
            if (x <= Log2Min)
            {
                return neg ? Fp.MaxValue : Fp.Zero;
            }

            /* The algorithm is based on the power series for exp(x):
             * http://en.wikipedia.org/wiki/Exponential_function#Formal_definition
             *
             * From term n, we get term n+1 by multiplying with x/n.
             * When the sum term drops to zero, we can stop summing.
             */

            int integerPart = (int)Floor(x);
            // Take fractional part of exponent
            x = new Fp(x.RawValue & 0x00000000FFFFFFFF);

            Fp result = Fp.One;
            Fp term = Fp.One;
            Fp ln2 = Ln2;
            
            int i = 1;
            while (term.RawValue != 0)
            {
                term = FastMul(FastMul(x, term), ln2) / (Fp)i;
                result += term;
                i++;
            }

            result = new Fp(result.RawValue << integerPart);
            if (neg)
            {
                result = Fp.One / result;
            }

            return result;
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// Provides about 5 digits of accuracy for the result.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// The base was zero, with a negative exponent
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The base was negative, with a non-zero exponent
        /// </exception>
        public static Fp Pow(Fp b, Fp exp)
        {
            if (b == Fp.One)
            {
                return Fp.One;
            }
            if (exp.RawValue == 0)
            {
                return Fp.One;
            }
            if (b.RawValue == 0)
            {
                if (exp.RawValue < 0)
                {
                    throw new DivideByZeroException();
                }
                return Fp.Zero;
            }

            Fp log2 = Log2(b);
            return Pow2(exp * log2);
        }
        
        /// <summary>
        /// Returns the base-2 logarithm of a specified number.
        /// Provides at least 9 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        public static Fp Log2(Fp x)
        {
            if (x.RawValue <= 0)
            {
                throw new ArgumentOutOfRangeException("Non-positive value passed to Ln", "x");
            }

            // This implementation is based on Clay. S. Turner's fast binary logarithm
            // algorithm (C. S. Turner,  "A Fast Binary Logarithm Algorithm", IEEE Signal
            //     Processing Mag., pp. 124,140, Sep. 2010.)

            long b = 1U << (Fp.FractionalPlaces - 1);
            long y = 0;

            long rawX = x.RawValue;
            while (rawX < Fp.InternalOne)
            {
                rawX <<= 1;
                y -= Fp.InternalOne;
            }

            while (rawX >= (Fp.InternalOne << 1))
            {
                rawX >>= 1;
                y += Fp.InternalOne;
            }

            var z = new Fp(rawX);

            for (int i = 0; i < Fp.FractionalPlaces; i++)
            {
                z = FastMul(z, z);
                if (z.RawValue >= (Fp.InternalOne << 1))
                {
                    z = new Fp(z.RawValue >> 1);
                    y += b;
                }
                b >>= 1;
            }

            return new Fp(y);
        }
        
        /// <summary>
        /// Returns the natural logarithm of a specified number.
        /// Provides at least 7 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        public static Fp Ln(Fp x)
        {
            return FastMul(Log2(x), Ln2);
        }

        /// <summary>
        /// Returns the minimum of two specified numbers.
        /// If the numbers are the same, it will return b.
        /// </summary>
        public static Fp Min(Fp a, Fp b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Returns the maximum of two specified numbers.
        /// If the numbers are the same, it will return b.
        /// </summary>
        public static Fp Max(Fp a, Fp b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the clamped value between 0 and 1
        /// </summary>
        public static Fp Clamp01(Fp value)
        {
            return Clamp(value, Fp.Zero, Fp.One);
        }
        
        /// <summary>
        /// Returns the clamped value between a maximum value and a minimum value
        /// </summary>
        public static Fp Clamp(Fp value, Fp min, Fp max)
        {
            if (value < min)
                value = min;

            else if (value > max)
                value = max;

            return value;
        }
    }
}
