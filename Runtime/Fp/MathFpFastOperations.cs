namespace FixedPoint
{
    public partial class MathFp
    {
     
        /// <summary>
        /// Adds x and y without performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        public static Fp FastAdd(Fp x, Fp y)
        {
            return new Fp(x.RawValue + y.RawValue);
        }
        
        /// <summary>
        /// Subtracts y from x without performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        public static Fp FastSub(Fp x, Fp y)
        {
            return new Fp(x.RawValue - y.RawValue);
        }
        
        /// <summary>
        /// Performs multiplication without checking for overflow.
        /// Useful for performance-critical code where the values are guaranteed not to cause overflow
        /// </summary>
        public static Fp FastMul(Fp x, Fp y)
        {

            var xl = x.RawValue;
            var yl = y.RawValue;

            var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
            var xhi = xl >> Fp.FractionalPlaces;
            var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
            var yhi = yl >> Fp.FractionalPlaces;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            var loResult = lolo >> Fp.FractionalPlaces;
            var midResult1 = lohi;
            var midResult2 = hilo;
            var hiResult = hihi << Fp.FractionalPlaces;

            var sum = (long)loResult + midResult1 + midResult2 + hiResult;
            return new Fp(sum);
        }
        
        /// <summary>
        /// Performs modulo as fast as possible; throws if x == MinValue and y == -1.
        /// Use the operator (%) for a more reliable but slower modulo.
        /// </summary>
        public static Fp FastMod(Fp x, Fp y)
        {
            return new Fp(x.RawValue % y.RawValue);
        }
    }
}