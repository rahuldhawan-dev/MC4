using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using NHibernate;
using StructureMap;
using System;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        CrewAssignmentRepositoryTest : MapCallMvcSecuredRepositoryTestBase<CrewAssignment, CrewAssignmentRepository,
            User>
    {
        #region Fields

        private DateTime _now;
        private TestDateTimeProvider _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now)));
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Private Methods

        private WorkOrder CreateWorkOrderWithTimeToComplete(decimal timeToComplete)
        {
            // Can't recreate these with times, it will just keep returning the first one created.
            var desc = (timeToComplete == 5m)
                ? GetFactory<ValveBoxRepairWorkDescriptionFactory>().Create(
                    new {TimeToComplete = timeToComplete})
                : GetFactory<MainBreakRepairWorkDescriptionFactory>().Create(
                    new {TimeToComplete = timeToComplete});
            return GetFactory<WorkOrderFactory>().Create(new {WorkDescription = desc});
        }

        #endregion

        [TestMethod]
        public void TestOpenCrewAssignmentsReturnsOpenCrewAssignments()
        {
            var now = new DateTime(2018, 5, 15, 12, 12, 00);
            _dateTimeProvider.SetNow(now);

            var workOrders = GetFactory<WorkOrderFactory>().CreateList(3);

            var crewAssignmentInvalid1 = GetFactory<CrewAssignmentFactory>()
               .Create(new {DateStarted = now, DateEnded = now.AddHours(1), WorkOrder = workOrders[0]});
            var crewAssignmentInvalid2 = GetFactory<CrewAssignmentFactory>()
               .Create(new {WorkOrder = workOrders[1]});
            var crewAssignmentValid = GetFactory<CrewAssignmentFactory>()
               .Create(new {DateStarted = now, WorkOrder = workOrders[2]});

            var result = Repository.OpenCompanyForcesCrewAssignments(24);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(workOrders[2].Id, result.First().WorkOrder.Id);
        }

        #region GetAllForCrewByDate

        [TestMethod]
        public void TestGetAllForCrewByDateReturnsOnlyCrewAssignmentsForTheGivenCrewAndDate()
        {
            var crew = GetFactory<CrewFactory>().Create();
            var crew2 = GetFactory<CrewFactory>().Create();
            var fact = GetFactory<CrewAssignmentFactory>();
            // Should be returned cause it's an exact match.
            fact.Create(new {Crew = crew, AssignedFor = _now});
            // Should be returned cause it's the same crew on the same date. Time is ignored.
            fact.Create(new {Crew = crew, AssignedFor = _now.AddMinutes(30)});
            // Should not be returned cause it's the wrong crew.
            fact.Create(new {Crew = crew2, AssignedFor = _now});
            // Should not be returned because it's the wrong date.
            fact.Create(new {Crew = crew, AssignedFor = _now.AddDays(1)});

            var crewOneAssignments = Repository.GetAllForCrewByDate(crew.Id, _now).ToList();

            Assert.IsTrue(crewOneAssignments.Count() == 2);
            foreach (var c in crewOneAssignments)
            {
                Assert.AreEqual(crew.Id, c.Crew.Id);
                Assert.IsNotNull(c.AssignedFor);
                Assert.AreEqual(_now.Date, c.AssignedFor.Date);
            }
        }

        #endregion

        #region GetCrewTimePercentagesByMonth

        [TestMethod]
        public void TestGetCrewTimePercentagesByMonthGroupsByMidnightDate()
        {
            var expectedDate = new DateTime(2011, 4, 24, 4, 4, 0);
            var expectedDateKey = new DateTime(2011, 4, 24);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new {Availability = 2.5m});
            GetFactory<CrewAssignmentFactory>()
               .Create(new {Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder});

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.Count == 1, "There should only be one group returned");
            Assert.IsTrue(result.ContainsKey(expectedDateKey),
                "There should be a start-of-day DateTime value of the passed in expectedDate");
        }

        [TestMethod]
        public void TestGetCrewTimePercentagesByMonthShouldAddWorkOrderTimeToCompleteAndDivideByCrewAvailability()
        {
            var expectedDate = new DateTime(2011, 4, 24, 4, 4, 0);
            var expectedDateKey = new DateTime(2011, 4, 24);
            var expectedWorkOrderTimeOne = 5m;
            var expectedWorkOrderTimeTwo = 10m;
            var expectedCrewAvailability = 2.5m;
            var workOrder = CreateWorkOrderWithTimeToComplete(expectedWorkOrderTimeOne);

            var crew = GetFactory<CrewFactory>().Create(new {Availability = expectedCrewAvailability});
            GetFactory<CrewAssignmentFactory>()
               .Create(new {Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder});

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.ContainsKey(expectedDateKey),
                "There should be a start-of-day DateTime value of the passed in expectedDate");
            Assert.AreEqual((expectedWorkOrderTimeOne / expectedCrewAvailability), result[expectedDateKey]);

            var anotherWorkOrder = CreateWorkOrderWithTimeToComplete(expectedWorkOrderTimeTwo);
            GetFactory<CrewAssignmentFactory>().Create(new
                {Crew = crew, AssignedFor = expectedDate.AddHours(1), WorkOrder = anotherWorkOrder});

            result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.AreEqual(((expectedWorkOrderTimeOne + expectedWorkOrderTimeTwo) / expectedCrewAvailability),
                result[expectedDateKey]);
        }

        [TestMethod]
        public void TestGetCrewTimePercentagesByMonthShouldReturnZeroWhenAvailabilityEquals0()
        {
            var expectedDate = new DateTime(2011, 4, 24, 4, 4, 0);
            var expectedDateKey = new DateTime(2011, 4, 24);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new {Availability = 0m});
            GetFactory<CrewAssignmentFactory>()
               .Create(new {Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder});

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.AreEqual(0, result[expectedDateKey]);
        }

        [TestMethod]
        public void TestGetCrewTimePercentagesByMonthShouldReturnZeroWhenTimeToCompleteEquals0()
        {
            var expectedDate = new DateTime(2011, 4, 24, 4, 4, 0);
            var expectedDateKey = new DateTime(2011, 4, 24);
            var workOrder = CreateWorkOrderWithTimeToComplete(0m);
            var crew = GetFactory<CrewFactory>().Create(new {Availability = 13413m});
            GetFactory<CrewAssignmentFactory>()
               .Create(new {Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder});

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.AreEqual(0, result[expectedDateKey]);
        }

        [TestMethod]
        public void TestGetCrewTimePercentageIncludesAbsoluteEndOfTheMonth()
        {
            var expectedDate = new DateTime(2011, 5, 1).AddTicks(-1);
            var expectedDateKey = new DateTime(2011, 4, 30);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new {Availability = 2.5m});
            GetFactory<CrewAssignmentFactory>()
               .Create(new {Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder});

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.Count == 1, "There should only be one group returned");
            Assert.IsTrue(result.ContainsKey(expectedDateKey),
                "There should be a start-of-day DateTime value of the passed in expectedDate");
        }

        [TestMethod]
        public void TestGetCrewTimePercentageIncludesAbsoluteBeginningOfTheMonth()
        {
            var expectedDate = new DateTime(2011, 5, 1);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new {Availability = 2.5m});
            GetFactory<CrewAssignmentFactory>()
               .Create(new {Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder});

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.Count == 1,
                "There should only be one group returned, but " + result.Count + " were returned");
            Assert.IsTrue(result.ContainsKey(expectedDate),
                "There should be a start-of-day DateTime value of the passed in expectedDate");
        }

        #endregion
    }
}
