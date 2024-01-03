using System;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SystemDeliveryEntry
{
    [TestClass]
    public class SystemDeliveryWorkDayEngineTest
    {
        [TestMethod]
        public void TestIsFourthBusinessDay()
        {
            var fourthBusinessDay = new DateTime(2023, 2, 6);
            var notFourthBusinessDay = new DateTime(2023, 2, 4);

            Assert.IsTrue(SystemDeliveryWorkDayEngine.IsFourthBusinessDay(fourthBusinessDay));
            Assert.IsFalse(SystemDeliveryWorkDayEngine.IsFourthBusinessDay(notFourthBusinessDay));
        }

        [TestMethod]
        public void TestIsSecondBusinessDay()
        {
            var secondBusinessDay = new DateTime(2023, 1, 4);
            var notSecondBusinessDay = new DateTime(2023, 1, 5);

            Assert.IsTrue(SystemDeliveryWorkDayEngine.IsSecondBusinessDay(secondBusinessDay));
            Assert.IsFalse(SystemDeliveryWorkDayEngine.IsSecondBusinessDay(notSecondBusinessDay));
        }
    }
}
