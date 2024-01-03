using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class DateTimeProviderTest
    {
        #region GetCurrentDate

        [TestMethod]
        public void TestGetCurrentDateReturnsCurrentDate()
        {
            MyAssert.AreClose(DateTime.Now,
                new DateTimeProvider().GetCurrentDate());
        }

        #endregion

        #region GetNext

        [TestMethod]
        public void TestGetNextReturnsForCurrentDayIfPossible()
        {
            var now = new DateTime(1984, 2, 21, 0, 0, 0);
            var expected = now.AddHours(12);

            Assert.AreEqual(expected, new TestDateTimeProvider(now).GetNext(12));

            now = new DateTime(1984, 2, 21, 12, 1, 0);
            expected = now.AddMinutes(1);

            Assert.AreEqual(expected, new TestDateTimeProvider(now).GetNext(12, 2));
        }

        [TestMethod]
        public void TestGetNextReturnsForNextDayIfHourHasAlreadyPassed()
        {
            var now = new DateTime(1984, 2, 21, 12, 0, 0);
            var expected = now.AddHours(23);

            Assert.AreEqual(expected, new TestDateTimeProvider(now).GetNext(11));
        }

        [TestMethod]
        public void TestGetNextReturnsForNextDayIfMinuteHasAlreadyPassed()
        {
            var now = new DateTime(1984, 2, 21, 11, 30, 0);
            var expected = now.AddHours(23).AddMinutes(30);

            Assert.AreEqual(expected, new TestDateTimeProvider(now).GetNext(11));
        }

        [TestMethod]
        public void TestGetNextReturnsForNextDayIfSecondHasAlreadyPassed()
        {
            var now = new DateTime(1984, 2, 21, 11, 0, 30);
            var expected = now.AddHours(23).AddMinutes(59).AddSeconds(30);

            Assert.AreEqual(expected, new TestDateTimeProvider(now).GetNext(11));
        }

        [TestMethod]
        public void TestGetNextForEndOfMonth()
        {
            var now = new DateTime(2015, 8, 31, 8, 30, 0);
            var expected = now.AddHours(24).AddMinutes(-30);

            Assert.AreEqual(expected, new TestDateTimeProvider(now).GetNext(8));
        }

        #endregion
    }
}
