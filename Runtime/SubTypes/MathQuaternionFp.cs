using UnityEngine;

namespace FixedPoint.SubTypes
{
    public class MathQuaternionFp
    {
        public static Fp Dot(QuaternionFp a, QuaternionFp b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }
    }
}