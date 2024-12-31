namespace FixedPoint
{
    public partial struct Fp
    {
        public static explicit operator Fp(long value)
        {
            return new Fp(value * InternalOne);
        }
        public static explicit operator long(Fp value)
        {
            return value._rawValue >> FractionalPlaces;
        }
        public static explicit operator Fp(float value)
        {
            return new Fp((long)(value * InternalOne));
        }
        public static explicit operator float(Fp value)
        {
            return (float)value._rawValue / InternalOne;
        }
        public static explicit operator Fp(double value)
        {
            return new Fp((long)(value * InternalOne));
        }
        public static explicit operator double(Fp value)
        {
            return (double)value._rawValue / InternalOne;
        }
        public static implicit operator Fp(decimal value)
        {
            return new Fp((long)(value * InternalOne));
        }
        public static implicit operator decimal(Fp value)
        {
            return (decimal)value._rawValue / InternalOne;
        }
    }
}
