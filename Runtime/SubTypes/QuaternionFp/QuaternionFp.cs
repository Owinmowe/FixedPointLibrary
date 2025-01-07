using System;
using System.Globalization;

namespace FixedPoint.SubTypes
{
    [Serializable]
    public partial struct QuaternionFp : IEquatable<QuaternionFp>, IFormattable
    {
        public Fp x;
        public Fp y;
        public Fp z;
        public Fp w;

        public readonly static QuaternionFp Identity = new QuaternionFp(Fp.Zero, Fp.Zero, Fp.Zero, Fp.One);

        public QuaternionFp(Fp x, Fp y, Fp z, Fp w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public QuaternionFp(UnityEngine.Quaternion quaternion)
        {
            this.x = new Fp(quaternion.x);
            this.y = new Fp(quaternion.y);
            this.z = new Fp(quaternion.z);
            this.w = new Fp(quaternion.w);
        }

        public static QuaternionFp operator *(QuaternionFp lhs, QuaternionFp rhs)
        {
            return new QuaternionFp(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vector3Fp operator *(QuaternionFp rotation, Vector3Fp point)
        {
            Fp num = rotation.x * new Fp(2f);
            Fp num2 = rotation.y * new Fp(2f);
            Fp num3 = rotation.z * new Fp(2f);
            Fp num4 = rotation.x * num;
            Fp num5 = rotation.y * num2;
            Fp num6 = rotation.z * num3;
            Fp num7 = rotation.x * num2;
            Fp num8 = rotation.x * num3;
            Fp num9 = rotation.y * num3;
            Fp num10 = rotation.w * num;
            Fp num11 = rotation.w * num2;
            Fp num12 = rotation.w * num3;
            Vector3Fp result = default;
            result.x = (Fp.One - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
            result.y = (num7 + num12) * point.x + (Fp.One - (num4 + num6)) * point.y + (num9 - num10) * point.z;
            result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (Fp.One - (num4 + num5)) * point.z;
            return result;
        }

        public void Normalize()
        {
            Fp num = MathFp.Sqrt(MathQuaternionFp.Dot(this, this));
            if (num <= 0)
            {
                this = QuaternionFp.Identity;
            }

            this = new QuaternionFp(this.x / num, this.y / num, this.z / num, this.w / num);
        }

        public static bool operator ==(QuaternionFp lhs, QuaternionFp rhs)
        {            
            return MathQuaternionFp.Dot(lhs, rhs) == Fp.One;
        }

        public static bool operator !=(QuaternionFp lhs, QuaternionFp rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }

        public override bool Equals(object other)
        {
            if (other is QuaternionFp other2)
            {
                return Equals(other2);
            }

            return false;
        }

        public bool Equals(QuaternionFp other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "F5";
            }

            if (formatProvider == null)
            {
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            }

            return $"({ x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)}, {w.ToString(format, formatProvider)})";
        }
    }
}