using System;
using MMSINC.Utilities.WorkDayEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.WorkDayEngine
{
    /// <summary>
    /// Summary description for WorkDayEngineTest
    /// </summary>
    [TestClass]
    public class WorkDayEngineTest
    {
        [TestMethod]
        public void TestDecrementingByNegativeValues()
        {
            // INVERSE OF TestGetDayInSameWeekWithNoHolidays:
            // Wednesday, November 5, 2008
            var date = new DateTime(2008, 11, 5);

            // Monday, November 3, 2008
            var expected = new DateTime(2008, 11, 3);
            var actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.DecrementByDays(
                    date, -1);

            Assert.AreEqual(expected, actual);

            // INVERSE OF TestGetDayInNextWeekWithNoHolidays:
            // Tuesday, November 11, 2008
            date = new DateTime(2008, 11, 11);
            // Monday, November 3, 2008
            expected = new DateTime(2008, 11, 3);
            actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.DecrementByDays(
                    date, -5);

            Assert.AreEqual(expected, actual);

            // INVERSE OF TestGetDayInSameWeekWithHoliday:
            // Thursday, February 14, 2008
            date = new DateTime(2008, 2, 14);
            // Monday, February 11, 2008
            expected = new DateTime(2008, 2, 11);
            // this should jump Lincoln's birthday
            actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.DecrementByDays(
                    date, -1);

            Assert.AreEqual(expected, actual);

            // INVERSE OF TestGetDayInNextWeekWithHoliday:
            date = new DateTime(2009, 1, 7);
            // Monday, December 29, 2008
            expected = new DateTime(2008, 12, 29);
            // this should jump both a weekend and New Year's Day
            actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.DecrementByDays(
                    date, -5);

            Assert.AreEqual(expected, actual);

            // INVERSE OF TestTakesNextYearsNewYearsDayIntoAccount:
            // Tuesday, January 4, 2011
            date = new DateTime(2011, 1, 4);
            // Tuesday, December 28, 2010
            expected = new DateTime(2010, 12, 28);
            actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.DecrementByDays(
                    date, -3);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIncrementByDaysEndsOnSkipDayByDefault()
        {
            // Thursday, no holidays
            var date = new DateTime(2011, 8, 11);
            // ends on Saturday
            var expected = date.AddDays(2);
            var actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(
                    date, 1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIncrementByDaysWillNotEndOnSkipDayIfSpecified()
        {
            // Thursday, no holidays
            var date = new DateTime(2011, 8, 11);
            // ends on Monday
            var expected = date.AddDays(4);
            var actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(
                    date, 1, true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetDayInSameWeekWithNoHolidays()
        {
            // Monday, November 3, 2008
            var date = new DateTime(2008, 11, 3);

            // Wednesday, November 5, 2008
            var expected = new DateTime(2008, 11, 5);
            var actual = WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(date, 1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetDayInNextWeekWithNoHolidays()
        {
            // Monday, November 3, 2008
            var date = new DateTime(2008, 11, 3);

            // Tuesday, November 11, 2008
            var expected = new DateTime(2008, 11, 11);
            var actual = WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(date, 5);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetDayInSameWeekWithHoliday()
        {
            // Monday, February 11, 2008
            var date = new DateTime(2008, 2, 11);

            // Thursday, February 14, 2008
            var expected = new DateTime(2008, 2, 14);
            // this should jump Lincoln's birthday
            var actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(
                    date, 1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetDayInNextWeekWithHoliday()
        {
            // Monday, December 29, 2008
            var date = new DateTime(2008, 12, 29);

            // Wednesday, January 5, 2009
            var expected = new DateTime(2009, 1, 7);
            // this should jump both a weekend and New Year's Day
            var actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(
                    date, 5);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTakesNextYearsNewYearsDayIntoAccount()
        {
            // Tuesday, December 28, 2010
            var date = new DateTime(2010, 12, 28);

            // Tuesday, January 4, 2011
            var expected = new DateTime(2011, 1, 4);

            var actual =
                WorkDayEngine<TestWorkDayEngineConfiguration>.IncrementByDays(
                    date, 3);

            Assert.AreEqual(expected, actual);
        }
    }

    internal class TestWorkDayEngineConfiguration : WorkDayEngineConfiguration
    {
        #region Properties

        public override bool UseChristmas
        {
            get { return true; }
        }

        public override bool UseNewYearsDay
        {
            get { return true; }
        }

        public override bool UseLincolnsBirthday
        {
            get { return true; }
        }

        #endregion
    }
}
