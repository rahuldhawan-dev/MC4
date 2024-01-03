using System;
using System.Linq;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    /// <summary>
    /// Summary description for DateTimeExtensionsTest
    /// </summary>
    [TestClass]
    public class DateTimeExtensionsTest
    {
        [TestMethod]
        public void TestGetNextDay()
        {
            var now = DateTime.Now;
            var tomorrow = now.AddDays(1);
            Assert.AreEqual(tomorrow,
                now.GetNextDay(),
                "DateTime.GetNextDay() is broken.");
        }

        [TestMethod]
        public void TestGetDayFromWeek()
        {
            // preliminaries:  if any of these break, so will the function under test
            Assert.AreEqual(0, (int)DayOfWeek.Sunday);
            Assert.AreEqual(1, (int)DayOfWeek.Monday);
            Assert.AreEqual(2, (int)DayOfWeek.Tuesday);
            Assert.AreEqual(3, (int)DayOfWeek.Wednesday);
            Assert.AreEqual(4, (int)DayOfWeek.Thursday);
            Assert.AreEqual(5, (int)DayOfWeek.Friday);
            Assert.AreEqual(6, (int)DayOfWeek.Saturday);

            // this was a monday:
            var monday = new DateTime(2009, 12, 7);

            Assert.AreEqual(monday,
                monday.GetDayFromWeek(DayOfWeek.Monday), "Error getting Monday");
            Assert.AreEqual(monday.AddDays(1),
                monday.GetDayFromWeek(DayOfWeek.Tuesday), "Error getting Tuesday");
            Assert.AreEqual(monday.AddDays(5),
                monday.GetDayFromWeek(DayOfWeek.Saturday), "Error getting Saturday");
            Assert.AreEqual(monday.AddDays(-1),
                monday.GetDayFromWeek(DayOfWeek.Sunday), "Error getting Sunday");
        }

        [TestMethod]
        public void TestGetNextWeek()
        {
            var now = DateTime.Now;
            var nextWeek = now.AddDays(7);
            Assert.AreEqual(nextWeek,
                now.GetNextWeek(),
                "DateTime.GetNextWeek() is broken.");
        }

        [TestMethod]
        public void TestGetPreviousDay()
        {
            var now = DateTime.Now;
            var yesterday = now.AddDays(-1);
            Assert.AreEqual(yesterday,
                now.GetPreviousDay(),
                "DateTime.GetPreviousDay() is broken.");
        }

        [TestMethod]
        public void TestGetBeginningOfMonth()
        {
            var expected = new DateTime(2009, 1, 1);
            var test = new DateTime(2009, 1, 20);

            Assert.AreEqual(expected, test.GetBeginningOfMonth());

            expected = new DateTime(2008, 2, 1);
            test = new DateTime(2008, 2, 29);

            Assert.AreEqual(expected, test.GetBeginningOfMonth());
        }

        [TestMethod]
        public void TestGetEndOfMonth()
        {
            var expected = new DateTime(2009, 1, 31);
            var test = new DateTime(2009, 1, 20);

            Assert.AreEqual(expected, test.GetEndOfMonth());

            expected = new DateTime(2008, 2, 29);
            test = new DateTime(2008, 2, 21);

            Assert.AreEqual(expected, test.GetEndOfMonth());

            expected = new DateTime(2023, 8, 31);
            test = new DateTime(2023, 8, 31);

            Assert.AreEqual(expected, test.GetEndOfMonth());
        }

        [TestMethod]
        public void TestSubtractYears()
        {
            var now = DateTime.Now;
            var lastYear = new DateTime(now.Year - 1, now.Month, now.Month,
                now.Hour, now.Minute, now.Second,
                now.Millisecond,
                now.Kind);
            Assert.AreEqual(lastYear,
                now.SubtractYears(1));
        }

        [TestMethod]
        public void TestIsWeekendDay()
        {
            // Friday, November 7, 2008
            var date = new DateTime(2008, 11, 7);
            Assert.IsFalse(date.IsWeekendDay());

            // Saturday, November 8, 2008
            date = new DateTime(2008, 11, 8);
            Assert.IsTrue(date.IsWeekendDay());

            // Sunday, November 9, 2008
            date = new DateTime(2008, 11, 9);
            Assert.IsTrue(date.IsWeekendDay());

            // Monday, November 10, 2008
            date = new DateTime(2008, 11, 10);
            Assert.IsFalse(date.IsWeekendDay());
        }

        [TestMethod]
        public void TestNext()
        {
            var expected = new DateTime(2022, 10, 24);
            var test = new DateTime(2022, 10, 20);

            Assert.AreEqual(expected, test.Next(DayOfWeek.Monday));

            expected = new DateTime(2022, 10, 28);
            test = new DateTime(2022, 10, 21);

            Assert.AreEqual(expected, test.Next(DayOfWeek.Friday));
        }

        [TestMethod]
        public void TestNextReturnsSaturday()
        {
            var expected = new DateTime(2022, 10, 15);
            var test = new DateTime(2022, 10, 14);

            Assert.AreEqual(expected, test.Next(DayOfWeek.Saturday));
        }

        [TestMethod]
        public void TestNextReturnsSunday()
        {
            var expected = new DateTime(2022, 10, 16);
            var test = new DateTime(2022, 10, 14);

            Assert.AreEqual(expected, test.Next(DayOfWeek.Sunday));
        }

        [TestMethod]
        public void TestIsBetweenReturnsTrueWhenDateIsBetweenStartAndEnd()
        {
            var date = DateTime.Today.GetNextDay();
            var start = DateTime.Today;
            var end = start.GetNextWeek();

            Assert.IsTrue(date.IsBetween(start, end));
        }

        [TestMethod]
        public void TestIsBetweenReturnFalseWhenDateIsBeforeStart()
        {
            var date = DateTime.Today;
            var start = date.GetNextWeek();
            var end = start.GetNextWeek();

            Assert.IsFalse(date.IsBetween(start, end));
        }

        [TestMethod]
        public void TestIsBetweenReturnsFalseWhenDateIsAfterEnd()
        {
            var date = DateTime.Today.GetNextWeek().GetNextDay();
            var start = DateTime.Today;
            var end = start.GetNextWeek();

            Assert.IsFalse(date.IsBetween(start, end));
        }

        [TestMethod]
        public void TestIsBetweenReturnsTrueWhenDateEqualsStartAndTestIsInclusive()
        {
            var date = DateTime.Today;
            var start = date;
            var end = start.GetNextWeek();

            Assert.IsTrue(date.IsBetween(start, end, true));
        }

        [TestMethod]
        public void TestIsBetweenReturnsTrueWhenDateEqualsEndAndTestIsInclusive()
        {
            var date = DateTime.Today.GetNextWeek();
            var start = DateTime.Today;
            var end = date;

            Assert.IsTrue(date.IsBetween(start, end, true));
        }

        [TestMethod]
        public void TestIsBetweenReturnsFalseWhenDateEqualsStartAndTestIsExclusive()
        {
            var date = DateTime.Today;
            var start = date;
            var end = start.GetNextWeek();

            Assert.IsFalse(date.IsBetween(start, end, false));
        }

        [TestMethod]
        public void TestIsBetweenReturnsFalseWhenDateEqualsEndAndTestIsExclusive()
        {
            var date = DateTime.Today.GetNextWeek();
            var start = DateTime.Today;
            var end = date;

            Assert.IsFalse(date.IsBetween(start, end, false));
        }

        [TestMethod]
        public void TestIsBetweenUsesInclusiveRangeByDefault()
        {
            var start = DateTime.Today;
            var end = start.GetNextWeek();

            Assert.IsTrue(start.IsBetween(start, end));
            Assert.IsTrue(end.IsBetween(start, end));
        }

        [TestMethod]
        public void TestIsBetweenThrowsExceptionWhenEndLiesChronologicallyBeforeStart()
        {
            var start = DateTime.Today;
            var end = start.SubtractYears(1);
            bool throwAway;

            MyAssert.Throws(() => throwAway = start.IsBetween(start, end),
                typeof(ArgumentException));
        }

        [TestMethod]
        public void TestBeginningOfDayRetursTheBeginningOfTheDay()
        {
            var expected = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var date = DateTime.Now;

            Assert.AreEqual(expected, date.BeginningOfDay());

            expected = new DateTime(2008, 10, 1, 0, 0, 0, 0);
            date = new DateTime(2008, 10, 1, 10, 10, 10, 10);

            Assert.AreEqual(expected, date.BeginningOfDay());
        }

        [TestMethod]
        public void TestEndOfDayRetursTheEndOfTheDay()
        {
            var expected = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
            var date = DateTime.Now;

            Assert.AreEqual(expected, date.EndOfDay());

            expected = new DateTime(2008, 10, 1, 23, 59, 59, 999);
            date = new DateTime(2008, 10, 1, 10, 10, 10, 10);

            Assert.AreEqual(expected, date.EndOfDay());
        }

        [TestMethod]
        public void TestToSixthReturnsCorrectEveryOtherMonth()
        {
            Assert.AreEqual(1, new DateTime(2013, 1, 1).GetYearSixth());
            Assert.AreEqual(1, new DateTime(2013, 2, 1).GetYearSixth());

            Assert.AreEqual(2, new DateTime(2013, 3, 1).GetYearSixth());
            Assert.AreEqual(2, new DateTime(2013, 4, 1).GetYearSixth());

            Assert.AreEqual(3, new DateTime(2013, 5, 1).GetYearSixth());
            Assert.AreEqual(3, new DateTime(2013, 6, 1).GetYearSixth());

            Assert.AreEqual(4, new DateTime(2013, 7, 1).GetYearSixth());
            Assert.AreEqual(4, new DateTime(2013, 8, 1).GetYearSixth());

            Assert.AreEqual(5, new DateTime(2013, 9, 1).GetYearSixth());
            Assert.AreEqual(5, new DateTime(2013, 10, 1).GetYearSixth());

            Assert.AreEqual(6, new DateTime(2013, 11, 1).GetYearSixth());
            Assert.AreEqual(6, new DateTime(2013, 12, 1).GetYearSixth());
        }

        [TestMethod]
        public void TestToQuarterReturnsCorrectQuarterForMonth()
        {
            Assert.AreEqual(1, new DateTime(2013, 1, 1).GetQuarter());
            Assert.AreEqual(1, new DateTime(2013, 2, 1).GetQuarter());
            Assert.AreEqual(1, new DateTime(2013, 3, 1).GetQuarter());

            Assert.AreEqual(2, new DateTime(2013, 4, 1).GetQuarter());
            Assert.AreEqual(2, new DateTime(2013, 5, 1).GetQuarter());
            Assert.AreEqual(2, new DateTime(2013, 6, 1).GetQuarter());

            Assert.AreEqual(3, new DateTime(2013, 7, 1).GetQuarter());
            Assert.AreEqual(3, new DateTime(2013, 8, 1).GetQuarter());
            Assert.AreEqual(3, new DateTime(2013, 9, 1).GetQuarter());

            Assert.AreEqual(4, new DateTime(2013, 10, 1).GetQuarter());
            Assert.AreEqual(4, new DateTime(2013, 11, 1).GetQuarter());
            Assert.AreEqual(4, new DateTime(2013, 12, 1).GetQuarter());
        }

        [TestMethod]
        public void TestToThirdReturnsCorrectThirdForMonth()
        {
            Assert.AreEqual(1, new DateTime(2013, 1, 1).GetYearThird());
            Assert.AreEqual(1, new DateTime(2013, 2, 1).GetYearThird());
            Assert.AreEqual(1, new DateTime(2013, 3, 1).GetYearThird());
            Assert.AreEqual(1, new DateTime(2013, 4, 1).GetYearThird());

            Assert.AreEqual(2, new DateTime(2013, 5, 1).GetYearThird());
            Assert.AreEqual(2, new DateTime(2013, 6, 1).GetYearThird());
            Assert.AreEqual(2, new DateTime(2013, 7, 1).GetYearThird());
            Assert.AreEqual(2, new DateTime(2013, 8, 1).GetYearThird());

            Assert.AreEqual(3, new DateTime(2013, 9, 1).GetYearThird());
            Assert.AreEqual(3, new DateTime(2013, 10, 1).GetYearThird());
            Assert.AreEqual(3, new DateTime(2013, 11, 1).GetYearThird());
            Assert.AreEqual(3, new DateTime(2013, 12, 1).GetYearThird());
        }

        [TestMethod]
        public void TestSecondsSinceEpochReturnsThatThingItSaysItShouldReturnInTheNameThatItHas()
        {
            Assert.AreEqual(446243400, new DateTime(1984, 2, 21, 20, 30, 0, DateTimeKind.Utc).SecondsSinceEpoch());
        }

        [TestMethod]
        public void TestMillisecondsSinceEpochReturnsThatThingItSaysItShouldReturnInTheNameThatItHas()
        {
            Assert.AreEqual(446243400000, new DateTime(1984, 2, 21, 20, 30, 0, DateTimeKind.Utc).MillisecondsSinceEpoch());
        }

        [TestMethod]
        public void TestDecade()
        {
            Assert.AreEqual(2000, new DateTime(2000, 1, 1).Decade());
            Assert.AreEqual(2000, new DateTime(2004, 1, 1).Decade());
            Assert.AreEqual(2000, new DateTime(2006, 1, 1).Decade());
            Assert.AreEqual(2000, new DateTime(2009, 1, 1).Decade());
            Assert.AreEqual(1990, new DateTime(1991, 1, 1).Decade());
            Assert.AreEqual(1990, new DateTime(1995, 1, 1).Decade());
            Assert.AreEqual(1900, new DateTime(1900, 1, 1).Decade());
            Assert.AreEqual(1900, new DateTime(1901, 1, 1).Decade());
            Assert.AreEqual(1890, new DateTime(1899, 1, 1).Decade());
            Assert.AreEqual(1890, new DateTime(1891, 1, 1).Decade());
        }

        [TestMethod]
        public void TestGetWeekOfMonthReturnsExpectedValues()
        {
            Assert.AreEqual(1, new DateTime(2018, 8, 1).GetWeekOfMonth());
            Assert.AreEqual(1, new DateTime(2018, 8, 2).GetWeekOfMonth());
            Assert.AreEqual(1, new DateTime(2018, 8, 3).GetWeekOfMonth());
            Assert.AreEqual(1, new DateTime(2018, 8, 4).GetWeekOfMonth());

            Assert.AreEqual(2, new DateTime(2018, 8, 5).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 8, 6).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 8, 7).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 8, 8).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 8, 9).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 8, 10).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 8, 11).GetWeekOfMonth());

            Assert.AreEqual(3, new DateTime(2018, 8, 12).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 8, 13).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 8, 14).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 8, 15).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 8, 16).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 8, 17).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 8, 18).GetWeekOfMonth());

            Assert.AreEqual(4, new DateTime(2018, 8, 19).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 8, 20).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 8, 21).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 8, 22).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 8, 23).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 8, 24).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 8, 25).GetWeekOfMonth());

            Assert.AreEqual(5, new DateTime(2018, 8, 26).GetWeekOfMonth());
            Assert.AreEqual(5, new DateTime(2018, 8, 27).GetWeekOfMonth());
            Assert.AreEqual(5, new DateTime(2018, 8, 28).GetWeekOfMonth());
            Assert.AreEqual(5, new DateTime(2018, 8, 29).GetWeekOfMonth());
            Assert.AreEqual(5, new DateTime(2018, 8, 30).GetWeekOfMonth());
            Assert.AreEqual(5, new DateTime(2018, 8, 31).GetWeekOfMonth());

            Assert.AreEqual(1, new DateTime(2018, 9, 1).GetWeekOfMonth());
        }

        [TestMethod]
        public void TestDoesMonthFollowQuarterEndMonth()
        {
            var year = DateTime.Now.Year;
            Assert.IsTrue(new DateTime(year, 1, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 2, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 3, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsTrue(new DateTime(year, 4, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 5, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 6, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsTrue(new DateTime(year, 7, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 8, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 9, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsTrue(new DateTime(year, 10, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 11, 1).DoesMonthFollowQuarterEndMonth());
            Assert.IsFalse(new DateTime(year, 12, 1).DoesMonthFollowQuarterEndMonth());
        }

        [TestMethod]
        public void TestGetDaysInMonthReturnsDaysInMonth()
        {
            var now = DateTime.Now;
            var isLeapYear = DateTime.IsLeapYear(now.Year);
            Assert.IsNotNull(now.GetDaysInMonth());
            Assert.AreEqual(31, new DateTime(now.Year, 1, 1).GetDaysInMonth().Count());
            Assert.AreEqual(isLeapYear ? 29 : 28, new DateTime(now.Year, 2, 1).GetDaysInMonth().Count());
            Assert.AreEqual(31, new DateTime(now.Year, 3, 1).GetDaysInMonth().Count());
            Assert.AreEqual(30, new DateTime(now.Year, 4, 1).GetDaysInMonth().Count());
            Assert.AreEqual(31, new DateTime(now.Year, 5, 1).GetDaysInMonth().Count());
            Assert.AreEqual(30, new DateTime(now.Year, 6, 1).GetDaysInMonth().Count());
            Assert.AreEqual(31, new DateTime(now.Year, 7, 1).GetDaysInMonth().Count());
            Assert.AreEqual(31, new DateTime(now.Year, 8, 1).GetDaysInMonth().Count());
            Assert.AreEqual(30, new DateTime(now.Year, 9, 1).GetDaysInMonth().Count());
            Assert.AreEqual(31, new DateTime(now.Year, 10, 1).GetDaysInMonth().Count());
            Assert.AreEqual(30, new DateTime(now.Year, 11, 1).GetDaysInMonth().Count());
            Assert.AreEqual(31, new DateTime(now.Year, 12, 1).GetDaysInMonth().Count());
        }
    }
}
