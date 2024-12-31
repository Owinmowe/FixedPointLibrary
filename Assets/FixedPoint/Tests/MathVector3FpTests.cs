using FixedPoint.SubTypes;
using NUnit.Framework;
using UnityEngine;

namespace FixedPoint.Tests
{
    public class MathVector3FpTests
    {
        [Test]
        public void ScaleTest()
        {
            Vector3 testVector = new Vector3(123.3f, 23, -12);
            Vector3Fp testFixedVector = new Vector3Fp(testVector);
            
            Vector3 testVectorScale = new Vector3(-12f, -23.12f, -123.2f);
            Vector3Fp testFixedVectorScale = new Vector3Fp(testVectorScale);
            
            Vector3 result = Vector3.Scale(testVector, testVectorScale) - (Vector3)MathVector3Fp.Scale(testFixedVector, testFixedVectorScale);
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
        
        [Test]
        public void CrossTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            Vector3 crossVector = Vector3.Cross(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Vector3Fp crossFixedVector = MathVector3Fp.Cross(testFixedVector1, testFixedVector2);

            Vector3 result = crossVector - (Vector3)crossFixedVector;
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
        
        [Test]
        public void DotTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            float dotValue = Vector3.Dot(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Fp dotValueFixed = MathVector3Fp.Dot(testFixedVector1, testFixedVector2);
            
            float result = Mathf.Abs(dotValue - (float)dotValueFixed);
            
            Assert.True(result < Mathf.Epsilon);
        }
        
        [Test]
        public void ProjectTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            Vector3 reflectedVector = Vector3.Project(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Vector3Fp reflectedFixedVector = MathVector3Fp.Project(testFixedVector1, testFixedVector2);

            Vector3 result = reflectedVector - (Vector3)reflectedFixedVector;
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
        
        [Test]
        public void ProjectOnPlaneTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            Vector3 reflectedVector = Vector3.ProjectOnPlane(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Vector3Fp reflectedFixedVector = MathVector3Fp.ProjectOnPlane(testFixedVector1, testFixedVector2);

            Vector3 result = reflectedVector - (Vector3)reflectedFixedVector;
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
        
        [Test]
        public void DistanceTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            float distanceValue = Vector3.Distance(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Fp fixedDistanceValue = MathVector3Fp.Distance(testFixedVector1, testFixedVector2);
            
            float result = Mathf.Abs(distanceValue - (float)fixedDistanceValue);
            
            Assert.True(result < Mathf.Epsilon);
        }
        
        [Test]
        public void LerpUnclampedTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            float testFloat1 = 12.542f;
            
            Vector3 lerpVector = Vector3.LerpUnclamped(testVector1, testVector2, testFloat1);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            Fp testFixedValue = new Fp(testFloat1);

            Vector3Fp lerpFixedVector = MathVector3Fp.LerpUnclamped(testFixedVector1, testFixedVector2, testFixedValue);
            
            Vector3 result = lerpVector - (Vector3)lerpFixedVector;
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
        
        [Test]
        public void MinTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            Vector3 minVector = Vector3.Min(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Vector3Fp minFixedVector = MathVector3Fp.Min(testFixedVector1, testFixedVector2);

            Vector3 result = minVector - (Vector3)minFixedVector;
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
        
        [Test]
        public void MaxTest()
        {
            Vector3 testVector1 = new Vector3(123.3f, 23, -12);
            Vector3 testVector2 = new Vector3(-12f, -23.12f, -123.2f);
            
            Vector3 maxVector = Vector3.Max(testVector1, testVector2);
            
            Vector3Fp testFixedVector1 = new Vector3Fp(testVector1);
            Vector3Fp testFixedVector2 = new Vector3Fp(testVector2);
            
            Vector3Fp maxFixedVector = MathVector3Fp.Max(testFixedVector1, testFixedVector2);

            Vector3 result = maxVector - (Vector3)maxFixedVector;
            
            Assert.True(result.magnitude < Mathf.Epsilon);
        }
    }
}
