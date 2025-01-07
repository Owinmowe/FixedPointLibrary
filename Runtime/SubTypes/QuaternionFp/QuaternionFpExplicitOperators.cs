using UnityEngine;

namespace FixedPoint.SubTypes
{
    public partial struct QuaternionFp
    {
        public static explicit operator QuaternionFp(Quaternion value)
        {
            return new QuaternionFp(new Fp(value.x), new Fp(value.y), new Fp(value.z), new Fp(value.w));
        }
        public static explicit operator Quaternion(QuaternionFp value)
        {
            return new Quaternion((float)value.x, (float)value.y, (float)value.z, (float)value.w);
        }
    }
}