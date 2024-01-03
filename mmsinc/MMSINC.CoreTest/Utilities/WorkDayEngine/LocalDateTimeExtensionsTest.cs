#if DEBUG
using System;
using MMSINC.Utilities.WorkDayEngine;
#endif
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.WorkDayEngine
{
    /// <summary>
    /// Summary description for LocalDateTimeExtensionsTest
    /// </summary>
    [TestClass]
    public class LocalDateTimeExtensionsTest
    {
#if DEBUG

        [TestMethod]
        public void TestIsHolidayReturnsFalseWhenDayIsNotHoliday()
        {
            // Monday, November 3, 2008
            var date = new DateTime(2008, 11, 3);

            Assert.IsFalse(date.IsHoliday(new TestWorkDayEngineConfiguration()));
        }

        [TestMethod]
        public void TestIsHolidayReturnsTrueWhenDayIsHoliday()
        {
            // Thursday, December 25, 2008
            var date = new DateTime(2008, 12, 25);

            Assert.IsTrue(date.IsHoliday(new TestWorkDayEngineConfiguration()));
        }

        [TestMethod]
        public void TestGetChristmasByYear()
        {
            //Christmas is Dec 25
            //Fri Dec 25 2009
            var expected = new DateTime(2009, 12, 25);
            var actual = new DateTime(2009, 7, 6).GetChristmasByYear();

            Assert.AreEqual(expected, actual);

            //Fri Dec 24 2010 observed
            expected = new DateTime(2010, 12, 24);
            actual = new DateTime(2010, 4, 16).GetChristmasByYear();

            Assert.AreEqual(expected, actual);

            //Mon Dec 26 2011 observed
            expected = new DateTime(2011, 12, 26);
            actual = new DateTime(2011, 4, 16).GetChristmasByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetColumbusDayByYear()
        {
            //Columbus day is the second Monday in October
            var expected = new DateTime(2008, 10, 13);
            var actual = new DateTime(2008, 1, 1).GetColumbusDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 10, 12);
            actual = new DateTime(2009, 10, 5).GetColumbusDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 10, 11);
            actual = new DateTime(2010, 6, 12).GetColumbusDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetEasterByYear()
        {
            // Sunday, March 23, 2008
            var expected = new DateTime(2008, 3, 23);
            var actual = new DateTime(2008, 1, 1).GetEasterByYear();

            Assert.AreEqual(expected, actual);

            // Sunday, April 12, 2009
            expected = new DateTime(2009, 4, 12);
            actual = new DateTime(2009, 1, 1).GetEasterByYear();

            Assert.AreEqual(expected, actual);

            // Sunday, April 4, 2010
            expected = new DateTime(2010, 4, 4);
            actual = new DateTime(2010, 1, 1).GetEasterByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetElectionDayByYear()
        {
            var expected = new DateTime(2008, 11, 4);
            var actual = new DateTime(2008, 1, 1).GetElectionDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 11, 3);
            actual = new DateTime(2009, 11, 28).GetElectionDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 11, 2);
            actual = new DateTime(2010, 4, 5).GetElectionDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetGoodFridayByYear()
        {
            // Friday, March 21, 2008
            var expected = new DateTime(2008, 3, 21);
            var actual = new DateTime(2008, 1, 1).GetGoodFridayByYear();

            Assert.AreEqual(expected, actual);

            // Friday, April 10, 2009
            expected = new DateTime(2009, 4, 10);
            actual = new DateTime(2009, 1, 1).GetGoodFridayByYear();

            Assert.AreEqual(expected, actual);

            // Friday, April 2, 2010
            expected = new DateTime(2010, 4, 2);
            actual = new DateTime(2010, 1, 1).GetGoodFridayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetIndependenceDayByYear()
        {
            //Independence day is July 4th

            //Fri, July 4th 2008
            var expected = new DateTime(2008, 7, 4);
            var actual = new DateTime(2008, 1, 1).GetIndependenceDayByYear();

            Assert.AreEqual(expected, actual);

            //Fri, July 3rd 2009 [observed]
            expected = new DateTime(2009, 7, 3);
            actual = new DateTime(2009, 9, 13).GetIndependenceDayByYear();

            Assert.AreEqual(expected, actual);

            //Mon, July 5th 2010 [observed]
            expected = new DateTime(2010, 7, 5);
            actual = new DateTime(2010, 6, 5).GetIndependenceDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetJuneteenthByYear()
        {
            var expected = new DateTime(2021, 6, 18);
            var actual = new DateTime(2021, 1, 1).GetJuneteenthByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2022, 6, 17);
            actual = new DateTime(2022, 1, 1).GetJuneteenthByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2023, 6, 16);
            actual = new DateTime(2023, 1, 1).GetJuneteenthByYear();

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void TestGetLaborDayByYear()
        {
            var expected = new DateTime(2008, 9, 1);
            var actual = new DateTime(2008, 1, 1).GetLaborDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 9, 7);
            actual = new DateTime(2009, 3, 15).GetLaborDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 9, 6);
            actual = new DateTime(2010, 8, 4).GetLaborDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetLincolnsBirthdayByYear()
        {
            //Lincoln's birthday is officially Feb 12
            //Feb 12, 2010
            var expected = new DateTime(2010, 2, 12);
            var actual =
                new DateTime(2010, 1, 1).GetLincolnsBirthdayByYear();

            Assert.AreEqual(expected, actual);

            //Feb 11, 2011 [observed]
            expected = new DateTime(2011, 2, 11);
            actual =
                new DateTime(2011, 10, 2).GetLincolnsBirthdayByYear();

            Assert.AreEqual(expected, actual);

            //Feb 13, 2012 [observed]
            expected = new DateTime(2012, 2, 13);
            actual =
                new DateTime(2012, 6, 17).GetLincolnsBirthdayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetMartinLutherKingDayByYear()
        {
            var expected = new DateTime(2008, 1, 21);
            var actual = new DateTime(2008, 1, 1).GetMartinLutherKingDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 1, 19);
            actual = new DateTime(2009, 1, 1).GetMartinLutherKingDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 1, 18);
            actual = new DateTime(2010, 1, 1).GetMartinLutherKingDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetMemorialDayByYear()
        {
            var expected = new DateTime(2008, 5, 26);
            var actual = new DateTime(2008, 1, 1).GetMemorialDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 5, 25);
            actual = new DateTime(2009, 2, 21).GetMemorialDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 5, 31);
            actual = new DateTime(2010, 7, 27).GetMemorialDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetNewYearsDayByYear()
        {
            var expected = new DateTime(2008, 1, 1);
            var actual = new DateTime(2008, 12, 1).GetNewYearsDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 1, 1);
            actual = new DateTime(2009, 6, 1).GetNewYearsDayByYear();

            Assert.AreEqual(expected, actual);

            //Fri, Jan 1
            expected = new DateTime(2010, 1, 1);
            actual = new DateTime(2010, 3, 5).GetNewYearsDayByYear();

            Assert.AreEqual(expected, actual);

            //Fri, Dec 31 [New Years Observed]
            expected = new DateTime(2010, 12, 31);
            actual = new DateTime(2011, 4, 14).GetNewYearsDayByYear();

            Assert.AreEqual(expected, actual);

            //Mon, Jan 2 [New Years Observed]
            expected = new DateTime(2012, 1, 2);
            actual = new DateTime(2012, 5, 18).GetNewYearsDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetPresidentsDayByYear()
        {
            var expected = new DateTime(2021, 2, 15);
            var actual = new DateTime(2021, 1, 1).GetPresidentsDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2022, 2, 21);
            actual = new DateTime(2022, 1, 1).GetPresidentsDayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2023, 2, 20);
            actual = new DateTime(2023, 1, 1).GetPresidentsDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetThanksgivingByYear()
        {
            var expected = new DateTime(2008, 11, 27);
            var actual = new DateTime(2008, 1, 1).GetThanksgivingByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 11, 26);
            actual = new DateTime(2009, 9, 23).GetThanksgivingByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 11, 25);
            actual = new DateTime(2010, 7, 20).GetThanksgivingByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetVeteransDayByYear()
        {
            //Veterens Day is Nov 11
            //Fri Nov 11, 2016
            var expected = new DateTime(2016, 11, 11);
            var actual = new DateTime(2016, 1, 1).GetVeteransDayByYear();

            Assert.AreEqual(expected, actual);

            //Fri Nov 10, 2017 observed
            expected = new DateTime(2017, 11, 10);
            actual = new DateTime(2017, 6, 23).GetVeteransDayByYear();

            Assert.AreEqual(expected, actual);

            //Mon Nov 12, 2018
            expected = new DateTime(2018, 11, 12);
            actual = new DateTime(2018, 9, 19).GetVeteransDayByYear();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetWashingtonsBirthdayByYear()
        {
            //Washington's birthday is the third monday in February
            var expected = new DateTime(2008, 2, 18);
            var actual = new DateTime(2008, 1, 1).GetWashingtonsBirthdayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2009, 2, 16);
            actual = new DateTime(2009, 5, 6).GetWashingtonsBirthdayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2010, 2, 15);
            actual = new DateTime(2010, 9, 12).GetWashingtonsBirthdayByYear();

            Assert.AreEqual(expected, actual);

            expected = new DateTime(2011, 2, 21);
            actual = new DateTime(2011, 1, 1).GetWashingtonsBirthdayByYear();

            Assert.AreEqual(expected, actual);
        }

#else
        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail("All tests have been written to run in DEBUG mode.  Please change your configuration and re-run the test suite.");
        }

#endif
    }
}
