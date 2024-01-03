using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ProductionWorkOrderFrequencyTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrderFrequencyTest>
    {
        [TestMethod]
        public void TestDailyGetFrequencyNextEndDateReturnsSameDay()
        {
            var startDate = new DateTime(2022, 9, 21);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.DAILY, startDate);
            Assert.AreEqual("9/21/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestWeeklyGetFrequencyNextEndDateReturnsNextSaturday()
        {
            var startDate = new DateTime(2022, 9, 18);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.WEEKLY, startDate);
            Assert.AreEqual(DayOfWeek.Saturday, resultDate.DayOfWeek);
            Assert.AreEqual("9/24/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestBiMonthlyGetFrequencyNextEndDateOnSecondReturnsFourteenth()
        {
            var startDate = new DateTime(2022, 9, 2);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.BI_MONTHLY, startDate);
            Assert.AreEqual("9/14/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestBiMonthlyGetFrequencyNextEndDateReturnsLastDayOfMonth()
        {
            var startDate = new DateTime(2022, 9, 16);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.BI_MONTHLY, startDate);
            Assert.AreEqual("9/30/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestMonthlyGetFrequencyNextEndDateReturnsLastDayOfMonth()
        {
            var startDate = new DateTime(2022, 9, 17);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.MONTHLY, startDate);
            Assert.AreEqual("9/30/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        [DataRow(1, "2/28/2022")]
        [DataRow(2, "2/28/2022")]
        [DataRow(3, "4/30/2022")]
        [DataRow(4, "4/30/2022")]
        [DataRow(5, "6/30/2022")]
        [DataRow(6, "6/30/2022")]
        [DataRow(7, "8/31/2022")]
        [DataRow(8, "8/31/2022")]
        [DataRow(9, "10/31/2022")]
        [DataRow(10, "10/31/2022")]
        [DataRow(11, "12/31/2022")]
        [DataRow(12, "12/31/2022")]
        public void TestEveryTwoMonthsGetFrequencyNextEndDateReturnsLastDayOfPeriod(int startMonth, string expectedDate)
        {
            var startDate = new DateTime(2022, startMonth, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_TWO_MONTHS, startDate);
            Assert.AreEqual(expectedDate, resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestQuarterlyGetFrequencyNextEndDateReturnsLastDayOfQuarter()
        {
            var startDate = new DateTime(2022, 10, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.QUARTERLY, startDate);
            Assert.AreEqual("12/31/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFourMonthsGetFrequencyNextEndDateReturnsLastDayOfPeriod()
        {
            var startDate = new DateTime(2022, 5, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_FOUR_MONTHS, startDate);
            Assert.AreEqual("8/31/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestBiAnnualGetFrequencyNextEndDateReturnsLastDayOfPeriod()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.BI_ANNUAL, startDate);
            Assert.AreEqual("12/31/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestAnnualGetFrequencyNextEndDateReturnsLastDayOfYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.ANNUAL, startDate);
            Assert.AreEqual("12/31/2022", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryTwoYearsGetFrequencyNextEndDateReturnsLastDayOfSecondYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_TWO_YEARS, startDate);
            Assert.AreEqual("12/31/2023", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryThreeYearsGetFrequencyNextEndDateReturnsLastDayOfThirdYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_THREE_YEARS, startDate);
            Assert.AreEqual("12/31/2024", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFourYearsGetFrequencyNextEndDateReturnsLastDayOfFourthYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_FOUR_YEARS, startDate);
            Assert.AreEqual("12/31/2025", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFiveYearsGetFrequencyNextEndDateReturnsLastDayOfFifthYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_FIVE_YEARS, startDate);
            Assert.AreEqual("12/31/2026", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryTenYearsGetFrequencyNextEndDateReturnsLastDayOfTenthYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_TEN_YEARS, startDate);
            Assert.AreEqual("12/31/2031", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFifteenYearsGetFrequencyNextEndDateReturnsLastDayOfFifeenthYear()
        {
            var startDate = new DateTime(2022, 9, 22);
            var resultDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(ProductionWorkOrderFrequency.Indices.EVERY_FIFTEEN_YEARS, startDate);
            Assert.AreEqual("12/31/2036", resultDate.ToShortDateString());
        }

        [TestMethod]
        public void TestAnnualGetFrequencyDateReturnsAnnualDates()
        {
            // Will generated a date; start dates are included in the date range
            var startDate = new DateTime(2022, 1, 1);
            // Will not generate a date; end dates are not included in the date range 
            var endDate = new DateTime(2028, 1, 1); 
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.ANNUAL, startDate, endDate);
            Assert.AreEqual(6, resultDates.Count());
            Assert.AreEqual("1/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2024", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2025", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2026", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("1/1/2027", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestBiAnnualGetFrequencyDatesReturnsBiAnnualDatesForTwoYears()
        {
            var startDate = new DateTime(2022, 2, 1);
            var endDate = new DateTime(2024, 2, 1);
            var target = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.BI_ANNUAL, Name = "BiAnnual" };
            var resultDates = target.GetFrequencyDates(startDate, endDate);
            Assert.AreEqual(4, resultDates.Count());
            Assert.AreEqual("7/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("7/1/2023", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2024", resultDates.ElementAt(3).ToShortDateString());
        }

        [TestMethod]
        public void TestBiMonthlyGetFrequencyDatesReturnsBiMonthlyDatesForDateRange()
        {
            var startDate = new DateTime(2022, 3, 3);
            var endDate = new DateTime(2022, 6, 20);
            var resultDates =
                ProductionWorkOrderFrequency.GetFrequencyDates(ProductionWorkOrderFrequency.Indices.BI_MONTHLY,
                    startDate, endDate);
            Assert.AreEqual(7, resultDates.Count());
            Assert.AreEqual("3/15/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("4/1/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("4/15/2022", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("5/1/2022", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("5/15/2022", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("6/1/2022", resultDates.ElementAt(5).ToShortDateString());
            Assert.AreEqual("6/15/2022", resultDates.ElementAt(6).ToShortDateString());
        }

        [TestMethod]
        public void TestDailyGetFrequencyDateReturnsDailyDates()
        {
            var startDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2022, 1, 7);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.DAILY, startDate, endDate);
            Assert.AreEqual(6, resultDates.Count());
            Assert.AreEqual("1/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/2/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/3/2022", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/4/2022", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/5/2022", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("1/6/2022", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFifteenYearsGetFrequencyDateReturnsEveryFifteenYearDates()
        {
            var startDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2072, 1, 1);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.EVERY_FIFTEEN_YEARS, startDate, endDate);
            Assert.AreEqual(3, resultDates.Count());
            Assert.AreEqual("1/1/2037", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2052", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2067", resultDates.ElementAt(2).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFiveYearsGetFrequencyDateReturnsEveryFiveYearDates()
        {
            var startDate = new DateTime(2022, 5, 31);
            var endDate = new DateTime(2050, 5, 31);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.EVERY_FIVE_YEARS, startDate, endDate);
            Assert.AreEqual(5, resultDates.Count());
            Assert.AreEqual("1/1/2027", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2032", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2037", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2042", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2047", resultDates.ElementAt(4).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFourMonthsGetFrequencyDatesReturnsEveryFourMonthDatesForOneYear()
        {
            var startDate = new DateTime(2022, 5, 1);
            var endDate = new DateTime(2023, 5, 1);
            var target = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.EVERY_FOUR_MONTHS, Name = "EveryFourMonths" };
            var resultDates = target.GetFrequencyDates(startDate, endDate);
            Assert.AreEqual(3, resultDates.Count());
            Assert.AreEqual("5/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("9/1/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(2).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryFourYearsGetFrequencyDateReturnsEveryFourYearDates()
        {
            var startDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2050, 1, 1);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.EVERY_FOUR_YEARS, startDate, endDate);
            Assert.AreEqual(6, resultDates.Count());
            Assert.AreEqual("1/1/2026", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2030", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2034", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2038", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2042", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("1/1/2046", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryTenYearsGetFrequencyDateReturnsEveryTenYearDates()
        {
            var startDate = new DateTime(2022, 12, 31);
            var endDate = new DateTime(2072, 12, 31);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.EVERY_TEN_YEARS, startDate, endDate);
            Assert.AreEqual(5, resultDates.Count());
            Assert.AreEqual("1/1/2032", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2042", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2052", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2062", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2072", resultDates.ElementAt(4).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryThreeYearsGetFrequencyDateReturnsEveryThreeYearDates()
        {
            var startDate = new DateTime(2022, 11, 1);
            var endDate = new DateTime(2040, 1, 1);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.EVERY_THREE_YEARS, startDate, endDate);
            Assert.AreEqual(5, resultDates.Count());
            Assert.AreEqual("1/1/2025", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2028", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2031", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2034", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2037", resultDates.ElementAt(4).ToShortDateString());
        }

        [TestMethod]
        public void TestEveryTwoYearsGetFrequencyDateReturnsEveryTwoYearDates()
        {
            var startDate = new DateTime(2022, 1, 2);
            var endDate = new DateTime(2034, 1, 2);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.EVERY_TWO_YEARS, startDate, endDate);
            Assert.AreEqual(6, resultDates.Count());
            Assert.AreEqual("1/1/2024", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2026", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2028", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("1/1/2030", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2032", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("1/1/2034", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestMonthlyGetFrequencyDateReturnsMonthlyDates()
        {
            var startDate = new DateTime(2022, 11, 30);
            var endDate = new DateTime(2023, 5, 31);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.MONTHLY, startDate, endDate);
            Assert.AreEqual(6, resultDates.Count());
            Assert.AreEqual("12/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("2/1/2023", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("3/1/2023", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("4/1/2023", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("5/1/2023", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestQuarterlyGetFrequencyDateReturnsQuarterlyDates()
        {
            var startDate = new DateTime(2022, 5, 31);
            var endDate = new DateTime(2024, 3, 31);
            var resultDates = ProductionWorkOrderFrequency.GetFrequencyDates(
                ProductionWorkOrderFrequency.Indices.QUARTERLY, startDate, endDate);
            Assert.AreEqual(7, resultDates.Count());
            Assert.AreEqual("7/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("10/1/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("4/1/2023", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("7/1/2023", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("10/1/2023", resultDates.ElementAt(5).ToShortDateString());
            Assert.AreEqual("1/1/2024", resultDates.ElementAt(6).ToShortDateString());
        }

        [TestMethod]
        public void TestWeeklyGetFrequencyDateReturnsWeeklyDatesWhenTodayIsNotSunday()
        {
            var startDate = new DateTime(2022, 8, 1);
            var endDate = new DateTime(2022, 9, 1);
            var resultDates =
                ProductionWorkOrderFrequency.GetFrequencyDates(ProductionWorkOrderFrequency.Indices.WEEKLY,
                    startDate, endDate);
            Assert.AreEqual(4, resultDates.Count());
            Assert.AreEqual("8/7/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(0).DayOfWeek);
            Assert.AreEqual("8/14/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(1).DayOfWeek);
            Assert.AreEqual("8/21/2022", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(2).DayOfWeek);
            Assert.AreEqual("8/28/2022", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(3).DayOfWeek);
        }

        [TestMethod]
        public void TestWeeklyGetFrequencyDateReturnsWeeklyDatesWhenTodayIsSunday()
        {
            var startDate = new DateTime(2023, 9, 3);
            var endDate = new DateTime(2023, 10, 3);
            var resultDates =
                ProductionWorkOrderFrequency.GetFrequencyDates(ProductionWorkOrderFrequency.Indices.WEEKLY,
                    startDate, endDate);
            Assert.AreEqual(5, resultDates.Count());
            Assert.AreEqual("9/3/2023", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(0).DayOfWeek);
            Assert.AreEqual("9/10/2023", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(1).DayOfWeek);
            Assert.AreEqual("9/17/2023", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(2).DayOfWeek);
            Assert.AreEqual("9/24/2023", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(3).DayOfWeek);
            Assert.AreEqual("10/1/2023", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual(DayOfWeek.Sunday, resultDates.ElementAt(4).DayOfWeek);
        }

        [TestMethod]
        public void TestEveryTwoMonthsGetFrequencyDatesReturnsEveryTwoMonthDatesForOneYear()
        {
            var startDate = new DateTime(2022, 5, 1);
            var endDate = new DateTime(2023, 5, 1);
            var target = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.EVERY_TWO_MONTHS, Name = "EveryTwoMonths" };
            var resultDates = target.GetFrequencyDates(startDate, endDate);
            Assert.AreEqual(6, resultDates.Count());
            Assert.AreEqual("5/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("7/1/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("9/1/2022", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("11/1/2022", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("3/1/2023", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestGetForecastDatesReturnsAllDatesFromInitialForecastRegardlessOfMinResultsParameter()
        {
            var startDate = new DateTime(2022, 5, 1);
            var target = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.EVERY_TWO_MONTHS, Name = "EveryTwoMonths", ForecastYearSpan = 1 };
            
            var resultDates = target.GetForecastDates(startDate);
            
            Assert.AreEqual(6, resultDates.Count());

            Assert.AreEqual("5/1/2022", resultDates.ElementAt(0).ToShortDateString());
            Assert.AreEqual("7/1/2022", resultDates.ElementAt(1).ToShortDateString());
            Assert.AreEqual("9/1/2022", resultDates.ElementAt(2).ToShortDateString());
            Assert.AreEqual("11/1/2022", resultDates.ElementAt(3).ToShortDateString());
            Assert.AreEqual("1/1/2023", resultDates.ElementAt(4).ToShortDateString());
            Assert.AreEqual("3/1/2023", resultDates.ElementAt(5).ToShortDateString());
        }

        [TestMethod]
        public void TestGetForecastDatesReturnsTheGivenNumberOfResultingDatesIfInitialForecastYieldsLessResults()
        {
            var startDate = new DateTime(2022, 5, 1);
            var target = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.EVERY_FIFTEEN_YEARS, Name = "EveryFifteenYears", ForecastYearSpan = 15 };
            const int numberOfResults = 3;

            var resultDates = target.GetForecastDates(startDate, numberOfResults);

            Assert.AreEqual(numberOfResults, resultDates.Count());
        }

        [TestMethod]
        public void TestGetForecastDatesThrowsAnExceptionIfForecastYearSpanIsNotAssigned()
        {
            var startDate = new DateTime(2022, 5, 1);
            var target = new ProductionWorkOrderFrequency { Id = ProductionWorkOrderFrequency.Indices.EVERY_FIFTEEN_YEARS, Name = "EveryFifteenYears" };

            // Need to force the resulting collection to enumerate using something like .Count() or .ToList()
            // because of deferred execution
            Assert.ThrowsException<Exception>(() => target.GetForecastDates(startDate).Count());
        }

        [TestMethod]
        public void TestToStringReturnsName()
        {
            var expected = "Test Name";
            var target = new ProductionWorkOrderFrequency { Name = expected };

            Assert.AreEqual(expected, target.ToString());
        }
    }
}