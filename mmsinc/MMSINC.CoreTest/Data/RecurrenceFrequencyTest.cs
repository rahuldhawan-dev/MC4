using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class RecurrenceFrequencyTest
    {
        [TestMethod]
        public void TestGetExpirationDateFromThrowsWhenUnitNotSet()
        {
            var target = new RecurrenceFrequency();

            MyAssert.Throws<InvalidOperationException>(() => target.GetExpirationDateFrom(DateTime.Now));
        }

        [TestMethod]
        public void TestGetExpirationDateFromReturnsYesterdayWhenOneDay()
        {
            var target = new RecurrenceFrequency {Frequency = 1, Unit = FrequencyUnit.Day};
            var now = DateTime.Now;

            Assert.AreEqual(now.AddDays(-1), target.GetExpirationDateFrom(now));
        }

        [TestMethod]
        public void TestGetExpirationDateFromReturnsYesterdayWhenTwoMonths()
        {
            var target = new RecurrenceFrequency {Frequency = 2, Unit = FrequencyUnit.Month};
            var now = DateTime.Now;

            Assert.AreEqual(now.AddMonths(-2), target.GetExpirationDateFrom(now));
        }

        [TestMethod]
        public void TestGetExpirationDateFromReturnsYesterdayWhenThreeWeeks()
        {
            var target = new RecurrenceFrequency {Frequency = 3, Unit = FrequencyUnit.Week};
            var now = DateTime.Now;

            Assert.AreEqual(now.AddWeeks(-3), target.GetExpirationDateFrom(now));
        }

        [TestMethod]
        public void TestGetExpirationDateFromReturnsYesterdayWhenFourYears()
        {
            var target = new RecurrenceFrequency {Frequency = 4, Unit = FrequencyUnit.Year};
            var now = DateTime.Now;

            Assert.AreEqual(now.AddYears(-4), target.GetExpirationDateFrom(now));
        }
    }
}
