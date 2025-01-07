using FixedPoint.SubTypes;
using NUnit.Framework;
using UnityEngine;

namespace FixedPoint.Tests
{
    public class MathQuaternionFpTests
    {
        [Test]
        public void DotTest()
        {
            Quaternion testQuaternion1 = Quaternion.Euler(new Vector3(2f, 92f, -23f));
            Quaternion testQuaternion2 = Quaternion.Euler(new Vector3(-754f, 163f, -23f));
            float testQuaternionResult = Quaternion.Dot(testQuaternion1, testQuaternion2);

            QuaternionFp testQuaternionFp1 = new QuaternionFp(testQuaternion1);
            QuaternionFp testQuaternionFp2 = new QuaternionFp(testQuaternion2);
            Fp testQuaternionFpResult = MathQuaternionFp.Dot(testQuaternionFp1, testQuaternionFp2);

            Assert.True((float)testQuaternionFpResult == testQuaternionResult);
        }
    }
}