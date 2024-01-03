using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class IncidentTest : InMemoryDatabaseTest<Incident>
    {
        #region Fields

        private Incident _target;
        private DateTime _now;
        private TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider);
        }

        [TestInitialize]
        public void InitializeTest()
        {

            _target = new Incident();
            _target.IncidentDate = new DateTime(2020, 2, 2);
            _container.BuildUp(_target);
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(TownFactory).Assembly);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDateReportedReturnsSameValueAsCreatedOn()
        {
            Assert.AreEqual(_target.CreatedAt, _target.DateRecorded);
            _target.CreatedAt = DateTime.Now;
            Assert.AreEqual(_target.CreatedAt, _target.DateRecorded);
        }

        [TestMethod]
        public void TestLostWorkDayReturnsTrueIfNumberOfLostWorkDaysIsntGivenEndDate()
        {
            _target.EmployeeAvailabilities.Clear();
            Assert.AreEqual(0, _target.NumberOfLostWorkDays);
            Assert.IsFalse(_target.LostWorkDay);

            var ilt = new IncidentEmployeeAvailability();
            _container.BuildUp(ilt);
            ilt.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            ilt.StartDate = DateTime.Today;
            _target.EmployeeAvailabilities.Add(ilt);

            Assert.AreEqual(1, _target.NumberOfLostWorkDays);
            Assert.IsTrue(_target.LostWorkDay);
        }

        [TestMethod]
        public void TestNumberOfRestrictiveDutyDaysReturnsCorrectValues()
        {
            _target.EmployeeAvailabilities.Clear();
            Assert.AreEqual(0, _target.NumberOfRestrictiveDutyDays);

            var ird = new IncidentEmployeeAvailability();
            _container.BuildUp(ird);
            ird.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            Assert.AreEqual(2, ird.EmployeeAvailabilityType.Id);
            ird.StartDate = _now.AddDays(-4);
            _target.EmployeeAvailabilities.Add(ird);

            Assert.AreEqual(5, _target.NumberOfRestrictiveDutyDays);

            ird.EndDate = _now.AddDays(1);
            Assert.AreEqual(6, _target.NumberOfRestrictiveDutyDays);
        }

        [TestMethod]
        public void TestLostTimeDurationReturnsCorrectValues()
        {
            _target.EmployeeAvailabilities.Clear();
            Assert.AreEqual(0, _target.NumberOfLostWorkDays);

            var ird = new IncidentEmployeeAvailability();
            _container.BuildUp(ird);
            ird.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);
            ird.StartDate = _now.AddDays(-4);

            Assert.AreEqual(5, _target.NumberOfLostWorkDays);

            ird.EndDate = _now.AddDays(1);
            Assert.AreEqual(6, _target.NumberOfLostWorkDays);
        }

        [TestMethod]
        public void TestGetLostWorkDaysBetweenDates()
        {
            var ird = new IncidentEmployeeAvailability();
            _container.BuildUp(ird);
            ird.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);
            ird.StartDate = new DateTime(2017, 2, 1);
            ird.EndDate = new DateTime(2017, 2, 7);

            // Searching the exact date range should work
            Assert.AreEqual(7,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2017, 2, 1), new DateTime(2017, 2, 7),
                    RangeOperator.Between));

            // Searching with the end date less than the actual end date should work
            Assert.AreEqual(6,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2017, 2, 1), new DateTime(2017, 2, 6),
                    RangeOperator.Between));

            // Searching with the start date greater than the end date should work
            Assert.AreEqual(6,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 7),
                    RangeOperator.Between));

            // Range that fits entirely inside the start/end date should work
            Assert.AreEqual(5,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 6),
                    RangeOperator.Between));

            // Range that includes the entirity of the start/end dates and then some should only include the range given 
            // by the StartDate and EndDate(ie this should be 7 days, not 9 days)
            Assert.AreEqual(7,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2017, 1, 31), new DateTime(2017, 2, 8),
                    RangeOperator.Between));

            // Test out of range returns 0 
            Assert.AreEqual(0,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2017, 1, 30), new DateTime(2017, 1, 31),
                    RangeOperator.Between));
        }

        [TestMethod]
        public void TestGetRestrictiveDutyDaysBetweenDates()
        {
            var ird = new IncidentEmployeeAvailability();
            _container.BuildUp(ird);
            ird.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);
            ird.StartDate = new DateTime(2017, 2, 1);
            ird.EndDate = new DateTime(2017, 2, 7);

            // Searching the exact date range should work
            Assert.AreEqual(7,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 1), new DateTime(2017, 2, 7),
                    RangeOperator.Between));

            // Searching with the end date less than the actual end date should work
            Assert.AreEqual(6,
                _target.GetRestrictiveDutyDaysBetweenDates(null, new DateTime(2017, 2, 6),
                    RangeOperator.LessThanOrEqualTo));
            Assert.AreEqual(6,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 1), new DateTime(2017, 2, 6),
                    RangeOperator.LessThanOrEqualTo));
            Assert.AreEqual(2,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 2),
                    RangeOperator.LessThan));
            Assert.AreEqual(1,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 1), new DateTime(2017, 2, 6),
                    RangeOperator.Equal));
            Assert.AreEqual(2,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 1), new DateTime(2017, 2, 6),
                    RangeOperator.GreaterThanOrEqualTo));

            // Searching with the start date greater than the end date should work
            Assert.AreEqual(6,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 8),
                    RangeOperator.Between));
            Assert.AreEqual(7,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 8),
                    RangeOperator.LessThan));
            Assert.AreEqual(1,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 7),
                    RangeOperator.GreaterThan));
            Assert.AreEqual(1,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 7),
                    RangeOperator.GreaterThanOrEqualTo));

            // Range that fits entirely inside the start/end date should work
            Assert.AreEqual(5,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 2, 2), new DateTime(2017, 2, 6),
                    RangeOperator.Between));

            // Range that includes the entirity of the start/end dates and then some should only include the range given 
            // by the StartDate and EndDate(ie this should be 7 days, not 9 days)
            Assert.AreEqual(7,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 1, 31), new DateTime(2017, 2, 8),
                    RangeOperator.Between));

            // Test out of range returns 0 
            Assert.AreEqual(0,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 1, 30), new DateTime(2017, 1, 31),
                    RangeOperator.Between));
            Assert.AreEqual(7,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2017, 1, 30), new DateTime(2017, 1, 31),
                    RangeOperator.GreaterThan));
        }

        [TestMethod]
        public void TestGetCumulativeLostWorkDaysThroughEndDate()
        {
            var ird = new IncidentEmployeeAvailability();
            ird.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);
            ird.StartDate = new DateTime(2017, 2, 1);
            ird.EndDate = new DateTime(2017, 2, 7);

            // Searching the exact date range should work
            Assert.AreEqual(7,
                _target.GetCumulativeLostWorkDaysThroughEndDate(new DateTime(2017, 2, 7), RangeOperator.Between));

            // EndDate is set but search is for a lesser value should still return the full duration.
            Assert.AreEqual(7,
                _target.GetCumulativeLostWorkDaysThroughEndDate(new DateTime(2017, 2, 6), RangeOperator.Between));

            // Searching past the EndDate should also return only the full duration, nothing more.
            Assert.AreEqual(7,
                _target.GetCumulativeLostWorkDaysThroughEndDate(new DateTime(2017, 2, 8), RangeOperator.Between));

            // if the EndDate isn't set, that should return duration based on the searched end date instead.
            ird.EndDate = null;
            Assert.AreEqual(180,
                _target.GetCumulativeLostWorkDaysThroughEndDate(new DateTime(2017, 2, 5), RangeOperator.Between));

            // Test cumulative when there are more than one result.
            var ird2 = new IncidentEmployeeAvailability();
            ird2.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird2);
            ird2.StartDate = new DateTime(2017, 2, 1);
            ird2.EndDate = new DateTime(2017, 2, 3);
            ird.EndDate = new DateTime(2017, 2, 7);

            Assert.AreEqual(10,
                _target.GetCumulativeLostWorkDaysThroughEndDate(new DateTime(2017, 2, 7), RangeOperator.Between));
        }

        [TestMethod]
        public void TestGetCumulativeRestrictiveDutyDaysThroughEndDate()
        {
            var ird = new IncidentEmployeeAvailability();
            ird.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);
            ird.StartDate = new DateTime(2017, 2, 1);
            ird.EndDate = new DateTime(2017, 2, 7);

            // Searching the exact date range should work
            Assert.AreEqual(7,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 7),
                    RangeOperator.Between));

            // EndDate is set but search is for a lesser value should still return the full duration.
            Assert.AreEqual(7,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 6),
                    RangeOperator.Between));

            // Searching past the EndDate should also return only the full duration, nothing more.
            Assert.AreEqual(7,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 8),
                    RangeOperator.Between));

            // if the EndDate isn't set, that should return duration based on the searched end date instead.
            ird.EndDate = null;
            Assert.AreEqual(5,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 5),
                    RangeOperator.Between));

            // Test cumulative when there are more than one result.
            var ird2 = new IncidentEmployeeAvailability();
            ird2.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird2);
            ird2.StartDate = new DateTime(2017, 2, 1);
            ird2.EndDate = new DateTime(2017, 2, 3);
            ird.EndDate = new DateTime(2017, 2, 7);

            Assert.AreEqual(10,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 7),
                    RangeOperator.Between));
        }

        [TestMethod]
        public void TestGetCumulativeRestrictiveDutyDaysThroughEndDateWhenTicketOpen()
        {
            var ird = new IncidentEmployeeAvailability();
            ird.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);
            ird.StartDate = new DateTime(2017, 2, 1);

            // Ticket's end date hasn't been set,  will return days from ticket's start through end date; these values are subject to change when the end date is set.
            Assert.AreEqual(7,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 7),
                    RangeOperator.Between));
            Assert.AreEqual(7,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 7),
                    RangeOperator.LessThan));
            Assert.AreEqual(7,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 7), RangeOperator.Equal));
            //180 in this case because greater than makes the given date the lowest value not the end date.
            Assert.AreEqual(180,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 5),
                    RangeOperator.GreaterThan));

            //Search for dates prior to when restrictive duty days started.
            Assert.AreEqual(0,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 1, 8),
                    RangeOperator.Between));

            // Test cumulative when there are more than one result.
            var ird2 = new IncidentEmployeeAvailability();
            ird2.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird2);
            ird2.StartDate = new DateTime(2017, 2, 5);

            Assert.AreEqual(10,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 2, 7),
                    RangeOperator.Between));
        }

        [TestMethod]
        public void TestGetDidIncidentHappenWithinTheRequestRangeForBetweenOperator()
        {
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(new DateTime(2020, 3, 1), new DateTime(2020, 11, 7),
                    RangeOperator.Between, _target.IncidentDate));
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(new DateTime(2020, 1, 1), new DateTime(2020, 1, 7),
                    RangeOperator.Between, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(new DateTime(2020, 1, 1), new DateTime(2020, 2, 7),
                    RangeOperator.Between, _target.IncidentDate));
        }

        [TestMethod]
        public void TestGetDidIncidentHappenWithinTheRequestRangeForEqualOperator()
        {
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 11, 7), RangeOperator.Equal,
                    _target.IncidentDate));
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 1, 7), RangeOperator.Equal,
                    _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 2), RangeOperator.Equal,
                    _target.IncidentDate));
        }

        [TestMethod]
        public void TestGetDidIncidentHappenWithinTheRequestRangeLessThanOrEqualOperator()
        {
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 1),
                    RangeOperator.LessThanOrEqualTo, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 2),
                    RangeOperator.LessThanOrEqualTo, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 11, 2),
                    RangeOperator.LessThanOrEqualTo, _target.IncidentDate));
        }

        [TestMethod]
        public void TestGetDidIncidentHappenWithinTheRequestRangeLessThanOperator()
        {
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 1),
                    RangeOperator.LessThan, _target.IncidentDate));
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 2),
                    RangeOperator.LessThan, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 11, 2),
                    RangeOperator.LessThan, _target.IncidentDate));
        }

        [TestMethod]
        public void TestGetDidIncidentHappenWithinTheRequestRangeGreaterThanOperator()
        {
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 11, 1),
                    RangeOperator.GreaterThan, _target.IncidentDate));
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 2),
                    RangeOperator.GreaterThan, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 1),
                    RangeOperator.GreaterThan, _target.IncidentDate));
        }

        [TestMethod]
        public void TestGetDidIncidentHappenWithinTheRequestRangeGreaterThanOrEqualToOperator()
        {
            Assert.AreEqual(false,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 11, 1),
                    RangeOperator.GreaterThanOrEqualTo, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 2),
                    RangeOperator.GreaterThanOrEqualTo, _target.IncidentDate));
            Assert.AreEqual(true,
                _target.GetDidIncidentHappenWithinTheRequestRange(null, new DateTime(2020, 2, 1),
                    RangeOperator.GreaterThanOrEqualTo, _target.IncidentDate));
        }

        [TestMethod]
        public void TestLostWorkDaysCapped()
        {
            var ilt = new IncidentEmployeeAvailability();
            _container.BuildUp(ilt);
            ilt.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ilt);

            ilt.StartDate = new DateTime(2019, 1, 1);
            ilt.EndDate = new DateTime(2019, 12, 30);

            // Lost time ranges of >180 return 180
            Assert.AreEqual(180,
                _target.GetLostWorkDaysBetweenDates(new DateTime(2019, 2, 1), new DateTime(2019, 10, 1),
                    RangeOperator.Between));
        }

        [TestMethod]
        public void TestRestrictiveWorkDaysCapped()
        {
            //Assemble
            var ird = new IncidentEmployeeAvailability();
            _container.BuildUp(ird);
            ird.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);

            ird.StartDate = new DateTime(2018, 1, 1);
            ird.EndDate = new DateTime(2018, 12, 27);

            //assert
            Assert.AreEqual(180,
                _target.GetRestrictiveDutyDaysBetweenDates(new DateTime(2018, 1, 1), new DateTime(2018, 11, 7),
                    RangeOperator.Between));
        }

        [TestMethod]
        public void TestCapsCumulativeRestrictiveDutyDays()
        {
            var ird = new IncidentEmployeeAvailability();
            ird.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird);

            ird.StartDate = new DateTime(2016, 1, 1);
            ird.EndDate = new DateTime(2016, 3, 31);

            Assert.AreEqual(91,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2017, 12, 7),
                    RangeOperator.Between));

            // Test cumulative when there are more than one result, and the sum is past the cap (so it's capped).
            var ird2 = new IncidentEmployeeAvailability();
            ird2.EmployeeAvailabilityType =
                GetFactory<RestrictiveDutyIncidentEmployeeAvailabilityTypeFactory>().Create();
            _target.EmployeeAvailabilities.Add(ird2);

            ird2.StartDate = new DateTime(2017, 1, 1);
            ird2.EndDate = new DateTime(2017, 4, 3);

            Assert.AreEqual(180,
                _target.GetCumulativeRestrictiveDutyDaysThroughEndDate(new DateTime(2018, 2, 7),
                    RangeOperator.Between));
        }

        #endregion
    }
}
