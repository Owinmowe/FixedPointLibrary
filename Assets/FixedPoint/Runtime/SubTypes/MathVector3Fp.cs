namespace FixedPoint.SubTypes
{
    public class MathVector3Fp
    {
        public static Vector3Fp Normalize(Vector3Fp vector3Fp)
        {
            return vector3Fp.Normalized;
        }
        
        public static Vector3Fp Scale(Vector3Fp a, Vector3Fp b)
        {
            return new Vector3Fp(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        
        public static Vector3Fp Cross(Vector3Fp lhs, Vector3Fp rhs)
        {
            return new Vector3Fp(
                lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.x * rhs.y - lhs.y * rhs.x);
        }
        
        public static Fp Dot(Vector3Fp lhs, Vector3Fp rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }
        
        public static Vector3Fp Project(Vector3Fp vector, Vector3Fp onNormal)
        {
            Fp num1 = Dot(onNormal, onNormal);
            if (num1 < 0)
                return Vector3Fp.Zero;
            
            Fp num2 = Dot(vector, onNormal);
            return new Vector3Fp(onNormal.x * num2 / num1, onNormal.y * num2 / num1, onNormal.z * num2 / num1);
        }
        
        public static Vector3Fp ProjectOnPlane(Vector3Fp vector, Vector3Fp planeNormal)
        {
            Fp num1 = Dot(planeNormal, planeNormal);
            if (num1 < 0)
                return vector;
            
            Fp num2 = Dot(vector, planeNormal);
            return new Vector3Fp(vector.x - planeNormal.x * num2 / num1, vector.y - planeNormal.y * num2 / num1, vector.z - planeNormal.z * num2 / num1);
        }
        
        public static Fp Distance(Vector3Fp a, Vector3Fp b)
        {
            Fp num1 = a.x - b.x;
            Fp num2 = a.y - b.y;
            Fp num3 = a.z - b.z;
            return MathFp.Sqrt( num1 * num1 + num2 * num2 + num3 * num3);
        }
        
        public static Fp Magnitude(Vector3Fp vector)
        {
            return vector.Magnitude;
        }
        
        public static Fp SqrtMagnitude(Vector3Fp vector)
        {
            return vector.SqrtMagnitude;
        }
        
        public static Vector3Fp Lerp(Vector3Fp a, Vector3Fp b, Fp t)
        {
            t = MathFp.Clamp01(t);
            return LerpUnclamped(a, b, t);
        }
        
        public static Vector3Fp LerpUnclamped(Vector3Fp a, Vector3Fp b, Fp t)
        {
            return new Vector3Fp(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }
        
        public static Vector3Fp Min(Vector3Fp lhs, Vector3Fp rhs)
        {
            return new Vector3Fp(MathFp.Min(lhs.x, rhs.x), MathFp.Min(lhs.y, rhs.y), MathFp.Min(lhs.z, rhs.z));
        }

        public static Vector3Fp Max(Vector3Fp lhs, Vector3Fp rhs)
        {
            return new Vector3Fp(MathFp.Max(lhs.x, rhs.x), MathFp.Max(lhs.y, rhs.y), MathFp.Max(lhs.z, rhs.z));
        }
        
        /*
        public static float Angle(Vector3Fp from, Vector3Fp to)
        {
            Fp num = MathFp.Sqrt(from.SqrtMagnitude * to.SqrtMagnitude);
            return (double) num < 1.0000000036274937E-15 ? 0.0f : (float) Math.Acos((double) Mathf.Clamp(Vector3.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }
        */
        
    }
}
