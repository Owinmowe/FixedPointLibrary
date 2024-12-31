using NUnit.Framework;
using UnityEngine;

namespace FixedPoint.Tests
{
    public class FpTests
    {
        [Test]
        public void CreationTest()
        {
            const int intTestValue = 10;
            const float floatTestValue = 10.1231231f; 
            
            Fp floatValueFixed = new Fp(floatTestValue);
            Fp intValueFixed = new Fp(intTestValue);
            
            Assert.True(floatValueFixed > intValueFixed);
        }
        
        [Test]
        public void AdditionTest()
        {
            const int intTestValue = 10;
            const float floatTestValue = 20.456f;
            
            Fp floatValueFixed = new Fp(floatTestValue);
            Fp intValueFixed = new Fp(intTestValue);
            Fp resultingSumFixed = floatValueFixed + intValueFixed;

            float result = (float)resultingSumFixed - (floatTestValue + intTestValue);
            
            Assert.False(Mathf.Abs(result) > float.Epsilon);
        }
        
        [Test]
        public void SubtractionTest()
        {
            const int intTestValue = 10;
            const float floatTestValue = 11.2345f;
            
            Fp intValueFixed = new Fp(intTestValue);
            Fp floatValueFixed = new Fp(floatTestValue);
            
            Fp resultingSubsFixed = intValueFixed - floatValueFixed;

            float result = (float)resultingSubsFixed - (intTestValue - floatTestValue);
            
            Assert.False(Mathf.Abs(result) > float.Epsilon);
        }
        
        [Test]
        public void MultiplicationTest()
        {
            const int intTestValue = 10;
            const float floatTestValue = 10.123423f;
            
            Fp intValueFixed = new Fp(intTestValue);
            Fp floatValueFixed = new Fp(floatTestValue);
            
            Fp resultingMultiplicationFixed = intValueFixed * floatValueFixed;

            float result = (float)resultingMultiplicationFixed - (intTestValue * floatTestValue);
            
            Assert.False(Mathf.Abs(result) > float.Epsilon);
        }
        
        [Test]
        public void DivisionTest()
        {
            const int intTestValue = 400000;
            const float floatTestValue = 123141235.2352134f;
            
            Fp intValueFixed = new Fp(intTestValue);
            Fp floatValueFixed = new Fp(floatTestValue);

            Fp resultingDivisionFixed = intValueFixed / floatValueFixed;
            
            float result = (float)resultingDivisionFixed - (intTestValue / floatTestValue);
            
            Assert.False(Mathf.Abs(result) > float.Epsilon);
        }

        [Test]
        public void ModuloTest()
        {
            const int intTestValue = 100;
            const float floatTestValue = 20f;
            
            Fp intValueFixed = new Fp(intTestValue);
            Fp floatValueFixed = new Fp(floatTestValue);

            int result = (int)(intValueFixed % floatValueFixed);

            Assert.True(result == 0);
        }
    }
}
