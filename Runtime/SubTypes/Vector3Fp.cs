using System;
using UnityEngine;

namespace FixedPoint.SubTypes
{
    [Serializable]
    public partial struct Vector3Fp : IEquatable<Vector3Fp>, IFormattable
    {
        /// <summary>
        ///   <para>X component of the vector.</para>
        /// </summary>
        public Fp x;
        /// <summary>
        ///   <para>Y component of the vector.</para>
        /// </summary>
        public Fp y;
        /// <summary>
        ///   <para>Z component of the vector.</para>
        /// </summary>
        public Fp z;

        public static readonly Vector3Fp Zero = new Vector3Fp(new Fp(0), new Fp(0), new Fp(0));
        public static readonly Vector3Fp One = new Vector3Fp(new Fp(1), new Fp(1), new Fp(1));
        public static readonly Vector3Fp Up = new Vector3Fp(new Fp(0), new Fp(1), new Fp(0));
        public static readonly Vector3Fp Down = new Vector3Fp(new Fp(0), new Fp(-1), new Fp(0));
        public static readonly Vector3Fp Left = new Vector3Fp(new Fp(-1), new Fp(0), new Fp(0));
        public static readonly Vector3Fp Right = new Vector3Fp(new Fp(1), new Fp(0), new Fp(0));
        public static readonly Vector3Fp Forward = new Vector3Fp(new Fp(0), new Fp(0), new Fp(1));
        public static readonly Vector3Fp Back = new Vector3Fp(new Fp(0), new Fp(0), new Fp(-1));
        
        public Vector3Fp(Fp x, Fp y, Fp z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public Vector3Fp(Vector3 vector3)
        {
            x = new Fp(vector3.x);
            y = new Fp(vector3.y);
            z = new Fp(vector3.z);
        }

        public Vector3Fp(float x, float y, float z)
        {
            this.x = new Fp(x);
            this.y = new Fp(y);
            this.z = new Fp(z);
        }
        
        public static Vector3Fp operator +(Vector3Fp a, Vector3Fp b) => new Vector3Fp(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vector3Fp operator -(Vector3Fp a, Vector3Fp b) => new Vector3Fp(a.x - b.x, a.y - b.y, a.z - b.z);

        public static Vector3Fp operator -(Vector3Fp a) => new Vector3Fp(-a.x, -a.y, -a.z);

        public static Vector3Fp operator *(Vector3Fp a, Fp d) => new Vector3Fp(a.x * d, a.y * d, a.z * d);

        public static Vector3Fp operator *(Fp d, Vector3Fp a) => new Vector3Fp(a.x * d, a.y * d, a.z * d);

        public static Vector3Fp operator /(Vector3Fp a, Fp d) => new Vector3Fp(a.x / d, a.y / d, a.z / d);

        public bool Equals(Vector3Fp other) => x == other.x && y == other.y && z == other.z;
        
        public static bool operator ==(Vector3Fp lhs, Vector3Fp rhs)
        {
            Fp num1 = lhs.x - rhs.x;
            Fp num2 = lhs.y - rhs.y;
            Fp num3 = lhs.z - rhs.z;
            
            return num1 + num2 + num3 == 0;
        }

        public static bool operator !=(Vector3Fp lhs, Vector3Fp rhs) => !(lhs == rhs);
        
        public Vector3Fp Normalized
        {
            get
            {
                Fp magnitudeValue = Magnitude;
                
                if (magnitudeValue > 0)
                    return this / magnitudeValue;
                
                return Zero;
            }
        }
        
        public Fp Magnitude 
        {
            get
            {
                Fp magnitude = MathFp.Sqrt(SqrtMagnitude);
                return magnitude;
            }
        }

        public Fp SqrtMagnitude
        {
            get
            {
                Fp sqrtMagnitude = x * x + y * y + z * z;
                return sqrtMagnitude;
            }
        }
        
        public override string ToString()
        {
            return $"{x.ToString()}, {y.ToString()}, {z.ToString()}";
        }
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $" {x.ToString("0.##########", formatProvider)}," +
                   $" {y.ToString("0.##########", formatProvider)}," +
                   $" {z.ToString("0.##########", formatProvider)}";
        }
        
        public override bool Equals(object obj)
        {
            return obj is Vector3Fp other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;
        }
    }
}
