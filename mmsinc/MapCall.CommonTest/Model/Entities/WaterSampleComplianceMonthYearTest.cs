using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WaterSampleComplianceMonthYearTest
    {
        [TestMethod]
        public void TestMonthAndYearHaveExpectedValues()
        {
            WaterSampleComplianceMonthYear target = null;

            Action<DateTime> setExpectedDate = (expectedDateTime) => {
                target = new WaterSampleComplianceMonthYear(expectedDateTime);
            };

            // Test that 2/1/2018 leads to 1/2018
            setExpectedDate(new DateTime(2018, 2, 1));
            Assert.AreEqual(1, target.Month, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2018, target.Year);

            // Test that 2/10/2018 leads to 1/2018
            setExpectedDate(new DateTime(2018, 2, 10));
            Assert.AreEqual(1, target.Month, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2018, target.Year);

            // Test that 2/11/2018 leads to 2/2018
            setExpectedDate(new DateTime(2018, 2, 11));
            Assert.AreEqual(2, target.Month,
                "Anytime after the 10th of the currenth month means certification is for the same month.");
            Assert.AreEqual(2018, target.Year);

            // Test that 3/10/2018 leads to 2/2018
            setExpectedDate(new DateTime(2018, 3, 10));
            Assert.AreEqual(2, target.Month, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2018, target.Year);

            // Test month/year when doing 1/1/2018 because that should then be 12/2017
            setExpectedDate(new DateTime(2018, 1, 1));
            Assert.AreEqual(12, target.Month, "Prior to the 11th of the month, certification is for the month before.");
            Assert.AreEqual(2017, target.Year, "The month was also last year so it should be last year in this case.");
        }
    }
}
