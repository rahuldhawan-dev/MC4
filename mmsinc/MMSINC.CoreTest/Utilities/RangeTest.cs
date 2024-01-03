using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class RangeTest
    {
        [TestMethod]
        public void TestConstructorThrowsArgumentOutOfRangeExceptionIfMinIsLessThanMax()
        {
            MyAssert.Throws<ArgumentOutOfRangeException>(() => new Range<int>(2, 1));
        }

        [TestMethod]
        public void TestConstructorAllowsMinAndMaxToBeEqual()
        {
            MyAssert.DoesNotThrow(() => new Range<int>(1, 1));
        }

        [TestMethod]
        public void TestConstructorSetsMinValue()
        {
            Assert.AreEqual(1, new Range<int>(1, 2).MinValue);
        }

        [TestMethod]
        public void TestConstructorSetsMaxValue()
        {
            Assert.AreEqual(2, new Range<int>(1, 2).MaxValue);
        }

        [TestMethod]
        public void TestIsInRangeReturnsTrueForValuesGreaterThanMinValueAndLessThanMaxValue()
        {
            var range = new Range<int>(1, 5);
            Assert.IsTrue(range.IsInRange(2));
            Assert.IsTrue(range.IsInRange(3));
            Assert.IsTrue(range.IsInRange(4));
        }

        [TestMethod]
        public void TestIsInRangeReturnsTrueForValuesEqualToMinValue()
        {
            Assert.IsTrue(new Range<int>(1, 5).IsInRange(1));
        }

        [TestMethod]
        public void TestIsInRangeReturnsTrueForValuesEqualToMaxValue()
        {
            Assert.IsTrue(new Range<int>(1, 5).IsInRange(5));
        }

        [TestMethod]
        public void TestIsInRangeReturnsFalseForValuesLessThanMinValue()
        {
            Assert.IsFalse(new Range<int>(1, 5).IsInRange(0));
        }

        [TestMethod]
        public void TestIsInRangeReturnsFalseForValuesGreaterThanMaxValue()
        {
            Assert.IsFalse(new Range<int>(1, 5).IsInRange(40));
        }
    }
}
