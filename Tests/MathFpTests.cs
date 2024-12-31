using NUnit.Framework;
using UnityEngine;

namespace FixedPoint.Tests
{
    public class MathFpTests
    {
        [Test]
        public void SignTest()
        {
            int intTestValue = -15;
            float floatTestValue = 123.2314f;

            Fp fixedIntValue = new Fp(intTestValue);
            Fp fixedFloatValue = new Fp(floatTestValue);
            
            Assert.True(MathFp.Sign(fixedIntValue) < 0 && MathFp.Sign(fixedFloatValue) > 0);
        }
        
        [Test]
        public void AbsTest()
        {
            float floatTestValue = 123.2314f;

            Fp fixedFloatValue = new Fp(floatTestValue);
            Fp resultValue = MathFp.Abs(fixedFloatValue);
            
            Assert.True(resultValue > 0);
        }
        
        [Test]
        public void FloorTest()
        {
            int intTestValue = 123;
            float floatTestValue = 123.7314f;
            
            Fp fixedFloatValue = new Fp(floatTestValue);
            Fp resultValue = MathFp.Floor(fixedFloatValue);
            
            Assert.True((int)resultValue == intTestValue);
        }
        
        [Test]
        public void CeilingTest()
        {
            int intTestValue = 124;
            float floatTestValue = 123.2314f;

            Fp fixedFloatValue = new Fp(floatTestValue);
            Fp resultValue = MathFp.Ceiling(fixedFloatValue);
            
            Assert.True((int)resultValue == intTestValue);
        }
        
        [Test]
        public void RoundTest()
        {
            int intTestValue = 124;
            float floatTestValue = 123.6314f;

            Fp fixedFloatValue = new Fp(floatTestValue);
            Fp resultValue = MathFp.Round(fixedFloatValue);
            
            Assert.True((int)resultValue == intTestValue);
        }

        [Test]
        public void TruncateTest()
        {
            float floatTestValue = 123.2314f;
            int intTestValue = 123;
            
            Fp fixedFloatValue = new Fp(floatTestValue);
            Fp resultValue = MathFp.Truncate(fixedFloatValue);
            
            Assert.True((int)resultValue == intTestValue);
        }
        
        [Test]
        public void SqrtTest()
        {
            float floatTestValue = 154.369f;
            float resultTestValue = Mathf.Sqrt(floatTestValue);
            
            Fp floatValueFixed = new Fp(floatTestValue);
            Fp resultValue = MathFp.Sqrt(floatValueFixed);
            
            Assert.True(Mathf.Abs((float)resultValue - resultTestValue) < float.Epsilon);
        }

        [Test]
        public void PowTest()
        {
            float floatTestValue = 22.542123f;
            float floatTestValue2 = 4.121233f;

            float resultTestValue = Mathf.Pow(floatTestValue, floatTestValue2);

            Fp floatValueFixed = new Fp(floatTestValue);
            Fp floatValueFixed2 = new Fp(floatTestValue2);

            Fp resultValue = MathFp.Pow(floatValueFixed, floatValueFixed2);

            float finalResult = Mathf.Abs((float)resultValue - resultTestValue);
            Assert.True(finalResult < float.Epsilon);
        }

        [Test]
        public void LnTest()
        {
            float floatTestValue = 5f;
            float resultTestValue = Mathf.Log(floatTestValue);

            Fp floatValueFixed = new Fp(floatTestValue);
            Fp resultValue = MathFp.Ln(floatValueFixed);
            
            float finalResult = Mathf.Abs((float)resultValue - resultTestValue);
            Assert.True(finalResult < float.Epsilon);
        }

        [Test] 
        public void MinTest()
        {
            Fp fixedTestValue1 = new Fp(1.23f);
            Fp fixedTestValue2 = new Fp(1.01f);

            Fp result = MathFp.Min(fixedTestValue1, fixedTestValue2);
            Assert.True(result == fixedTestValue2);
        }

        [Test]
        public void MaxTest()
        {
            Fp fixedTestValue1 = new Fp(1.23f);
            Fp fixedTestValue2 = new Fp(1.01f);

            Fp result = MathFp.Max(fixedTestValue1, fixedTestValue2);
            Assert.True(result == fixedTestValue1);
        }

        [Test]
        public void ClampTest()
        {
            Fp fixedTestValue1 = new Fp(1.23f);
            Fp fixedTestValue2 = new Fp(10.01f);
            Fp fixedTestValue3 = new Fp(5.01f);
            
            Fp fixedTestValue4 = new Fp(1.23f);
            Fp fixedTestValue5 = new Fp(0.01f);
            Fp fixedTestValue6 = new Fp(5.01f);

            Fp result1 = MathFp.Clamp(fixedTestValue2, fixedTestValue1, fixedTestValue3);
            Fp result2 = MathFp.Clamp(fixedTestValue5, fixedTestValue4, fixedTestValue6);
            
            Assert.True(result1 == fixedTestValue3 && result2 == fixedTestValue4);
        }
    }
}
