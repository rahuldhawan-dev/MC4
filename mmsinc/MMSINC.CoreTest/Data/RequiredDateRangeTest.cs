using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class RequiredDateRangeTest
    {
        [TestMethod]
        public void TestEndDateIsRequired()
        {
            var target = new RequiredDateRange();

            Assert.IsFalse(target.IsValid);
        }

        [TestMethod]
        public void TestValid()
        {
            var target = new RequiredDateRange {
                End = DateTime.Now,
                Operator = RangeOperator.GreaterThan
            };

            Assert.IsTrue(target.IsValid);
        }
    }
}
