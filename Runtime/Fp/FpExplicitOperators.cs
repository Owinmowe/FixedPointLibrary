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
            return value.RawValue >> FractionalPlaces;
        }
        public static explicit operator Fp(float value)
        {
            return new Fp((long)(value * InternalOne));
        }
        public static explicit operator float(Fp value)
        {
            return (float)value.RawValue / InternalOne;
        }
        public static explicit operator Fp(double value)
        {
            return new Fp((long)(value * InternalOne));
        }
        public static explicit operator double(Fp value)
        {
            return (double)value.RawValue / InternalOne;
        }
        public static implicit operator Fp(decimal value)
        {
            return new Fp((long)(value * InternalOne));
        }
        public static implicit operator decimal(Fp value)
        {
            return (decimal)value.RawValue / InternalOne;
        }
    }
}
