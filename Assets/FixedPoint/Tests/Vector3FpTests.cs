using FixedPoint.SubTypes;
using NUnit.Framework;
using UnityEngine;

namespace FixedPoint.Tests
{
    public class Vector3FpTests
    {
        [Test]
        public void CreationTest()
        {
            Vector3 testVector3 = new Vector3(13, 2.64f, -3.12f);
            
            Vector3Fp testVector3Fp1 = new Vector3Fp(testVector3.x, testVector3.y, testVector3.z);
            Vector3Fp testVector3Fp2 = new Vector3Fp(testVector3);
            Vector3Fp testVector3Fp3 = 
                new Vector3Fp(new Fp(testVector3.x), new Fp(testVector3.y), new Fp(testVector3.z));
            
            Assert.True(testVector3Fp1 == testVector3Fp2 && testVector3Fp2 == testVector3Fp3);
        }

        [Test]
        public void NormalizeTest()
        {
            Vector3 testVector3 = new Vector3(13, 2.64f, -3.12f);
            Vector3Fp testVector3Fp1 = (Vector3Fp)testVector3;

            float result = Mathf.Abs(testVector3.normalized.magnitude - (float)testVector3Fp1.Normalized.Magnitude);
            Assert.True(result < Mathf.Epsilon);
        }
        
        [Test]
        public void MagnitudeTest()
        {
            Vector3 testVector3 = new Vector3(-1243, 2.6421f, 13.12f);
            Vector3Fp testVector3Fp1 = (Vector3Fp)testVector3;
            
            Assert.True(Mathf.Abs(testVector3.magnitude - (float)testVector3Fp1.Magnitude) < Mathf.Epsilon);
        }
        
        [Test]
        public void SqrtMagnitudeTest()
        {
            Vector3 testVector3 = new Vector3(-12.43f, 2312f, -0.002f);
            Vector3Fp testVector3Fp1 = (Vector3Fp)testVector3;
            
            Assert.True(Mathf.Abs(testVector3.sqrMagnitude - (float)testVector3Fp1.SqrtMagnitude) < Mathf.Epsilon);
        }
    }
}
