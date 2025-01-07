using UnityEngine;

namespace FixedPoint.SubTypes
{
    public partial struct Vector3Fp
    {
        public static explicit operator Vector3Fp(Vector3 value)
        {
            return new Vector3Fp(new Fp(value.x), new Fp(value.y), new Fp(value.z));
        }
        public static explicit operator Vector3(Vector3Fp value)
        {
            return new Vector3((float)value.x, (float)value.y, (float)value.z);
        }
    }
}
