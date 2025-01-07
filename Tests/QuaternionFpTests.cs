using FixedPoint.SubTypes;
using NUnit.Framework;
using UnityEngine;

namespace FixedPoint.Tests
{
    public class QuaternionFpTests
    {
        [Test]
        public void CreationTest()
        {
            Quaternion testQuaternion = Quaternion.Euler(new Vector3(2f, 123f, 23f));
            QuaternionFp testQuaternionFp = new QuaternionFp(testQuaternion);

            Assert.True((Quaternion)testQuaternionFp == testQuaternion);
        }

        [Test]
        public void MultiplicationTest()
        {
            Quaternion testQuaternion1 = Quaternion.Euler(new Vector3(2f, 92f, -23f));
            Quaternion testQuaternion2 = Quaternion.Euler(new Vector3(-754f, 163f, -23f));
            Quaternion testQuaternionResult = testQuaternion1 * testQuaternion2;

            QuaternionFp testQuaternionFp1 = new QuaternionFp(testQuaternion1);
            QuaternionFp testQuaternionFp2 = new QuaternionFp(testQuaternion2);
            QuaternionFp testQuaternionFpResult = testQuaternionFp1 * testQuaternionFp2;

            Assert.True((Quaternion)testQuaternionFpResult == testQuaternionResult);
        }

        [Test]
        public void MultiplicationPointTest()
        {
            Quaternion testQuaternion = Quaternion.Euler(new Vector3(2f, 92f, -23f));
            Vector3 testVector3 = new Vector3(-754f, 163f, -23f);
            Vector3 testVector3Result = testQuaternion * testVector3;

            QuaternionFp testQuaternionFp = new QuaternionFp(testQuaternion);
            Vector3Fp testVector3Fp = new Vector3Fp(testVector3);
            Vector3Fp testVector3FpResult = testQuaternionFp * testVector3Fp;

            Assert.True((Vector3)testVector3FpResult == testVector3Result);
        }
    }
}