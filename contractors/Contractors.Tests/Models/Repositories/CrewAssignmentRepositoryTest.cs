using System;
using System.Linq;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using StructureMap;
using CrewAssignmentRepository = Contractors.Data.Models.Repositories.CrewAssignmentRepository;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class CrewAssignmentRepositoryTest : ContractorsControllerTestBase<CrewAssignment, TestCrewAssignmentRepository>
    {
        #region Fields

        private readonly DateTime _testDate = DateTime.Today;

        #endregion

        #region Setup/cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            Repository = _container.GetInstance<TestCrewAssignmentRepository>();
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
            return GetFactory<WorkOrderFactory>().Create(new { WorkDescription = desc });
        }

        #endregion

        #region Tests

        #region GetAll

        [TestMethod]
        public void TestLinqOnlyReturnsCrewAssignmentsForCrewsForTheCurrentUsersContractor()
        {
            var goodCrew = GetFactory<CrewFactory>().Create(new { Contractor = _currentUser.Contractor });
            var badContractor = GetFactory<ContractorFactory>().Create();
            var badCrew = GetFactory<CrewFactory>().Create(new { Contractor = badContractor });
            var goodAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = goodCrew, AssignedFor = _testDate });
            var badAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = badCrew, AssignedFor = _testDate });

            var all = Repository.GetAll();

            Assert.IsTrue(all.Where(ca => ca.Id == goodAss.Id).Any());
            Assert.IsFalse(all.Where(ca => ca.Id == badAss.Id).Any());
        }

        [TestMethod]
        public void TestCriteriaFiltersCrewAssignmentsByCurrentUsersContractor()
        {
            var goodCrew = GetFactory<CrewFactory>().Create(new { Contractor = _currentUser.Contractor });
            var badContractor = GetFactory<ContractorFactory>().Create();
            var badCrew = GetFactory<CrewFactory>().Create(new { Contractor = badContractor });
            var goodAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = goodCrew, AssignedFor = _testDate });
            var badAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = badCrew, AssignedFor = _testDate });

            var all = Repository.CriteriaTest.List<CrewAssignment>();
            Assert.IsTrue(all.Where(ca => ca.Id == goodAss.Id).Any());
            Assert.IsFalse(all.Where(ca => ca.Id == badAss.Id).Any());
        }

        [TestMethod]
        public void TestFindReturnsEntityIfItMatchesCurrentUserContractor()
        {
            var goodCrew = GetFactory<CrewFactory>().Create(new { Contractor = _currentUser.Contractor });
            var goodAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = goodCrew, AssignedFor = _testDate });
            var result = Repository.Find(goodAss.Id);
            Assert.AreEqual(goodAss.Id, result.Id);
        }

        [TestMethod]
        public void TestFindReturnsNullIfCrewAssignmentDoesNotMatchCurrentUserContractor()
        {
            var badContractor = GetFactory<ContractorFactory>().Create();
            var badCrew = GetFactory<CrewFactory>().Create(new { Contractor = badContractor });
            var badAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = badCrew, AssignedFor = _testDate });
            var result = Repository.Find(badAss.Id);
            Assert.IsNull(result);
        }

        #endregion

        #region GetAllForCrewByDate

        [TestMethod]
        public void TestGetAllForCrewByDateReturnsOnlyCrewAssignmentsForTheGivenCrewAndDate()
        {
            var crew = GetFactory<CrewFactory>().Create(new { Contractor = _currentUser.Contractor });
            var crew2 = GetFactory<CrewFactory>().Create(new { Contractor = _currentUser.Contractor });
            var fact = GetFactory<CrewAssignmentFactory>();
            // Should be returned cause it's an exact match.
            fact.Create(new { Crew = crew, AssignedFor = _testDate });
            // Should be returned cause it's the same crew on the same date. Time is ignored.
            fact.Create(new { Crew = crew, AssignedFor = _testDate.AddMinutes(30) });
            // Should not be returned cause it's the wrong crew.
            fact.Create(new { Crew = crew2, AssignedFor = _testDate });
            // Should not be returned because it's the wrong date.
            fact.Create(new { Crew = crew, AssignedFor = _testDate.AddDays(1) });

            var crewOneAssignments = Repository.GetAllForCrewByDate(crew.Id, _testDate).ToList();

            Assert.IsTrue(crewOneAssignments.Count() == 2);
            foreach (var c in crewOneAssignments)
            {
                Assert.AreEqual(crew.Id, c.Crew.Id);
                Assert.IsNotNull(c.AssignedFor);
                Assert.AreEqual(_testDate.Date, c.AssignedFor.Date);
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
            var crew = GetFactory<CrewFactory>().Create(new { Availability = 2.5m, Contractor = _currentUser.Contractor });
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder });

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.Count == 1, "There should only be one group returned");
            Assert.IsTrue(result.ContainsKey(expectedDateKey), "There should be a start-of-day DateTime value of the passed in expectedDate");
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

            var crew = GetFactory<CrewFactory>().Create(new { Availability = expectedCrewAvailability, Contractor = _currentUser.Contractor });
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder });

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.ContainsKey(expectedDateKey), "There should be a start-of-day DateTime value of the passed in expectedDate");
            Assert.AreEqual((expectedWorkOrderTimeOne / expectedCrewAvailability), result[expectedDateKey]);

            var anotherWorkOrder = CreateWorkOrderWithTimeToComplete(expectedWorkOrderTimeTwo);
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate.AddHours(1), WorkOrder = anotherWorkOrder });

            result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.AreEqual(((expectedWorkOrderTimeOne + expectedWorkOrderTimeTwo) / expectedCrewAvailability), result[expectedDateKey]);
        }

        [TestMethod]
        public void TestGetCrewTimePercentagesByMonthShouldReturnZeroWhenAvailabilityEquals0()
        {
            var expectedDate = new DateTime(2011, 4, 24, 4, 4, 0);
            var expectedDateKey = new DateTime(2011, 4, 24);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new { Availability = 0m, Contractor = _currentUser.Contractor });
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder });

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.AreEqual(0, result[expectedDateKey]);
        }

        [TestMethod]
        public void TestGetCrewTimePercentagesByMonthShouldReturnZeroWhenTimeToCompleteEquals0()
        {
            var expectedDate = new DateTime(2011, 4, 24, 4, 4, 0);
            var expectedDateKey = new DateTime(2011, 4, 24);
            var workOrder = CreateWorkOrderWithTimeToComplete(0m);
            var crew = GetFactory<CrewFactory>().Create(new { Availability = 13413m, Contractor = _currentUser.Contractor });
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder });

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.AreEqual(0, result[expectedDateKey]);
        }

        [TestMethod]
        public void TestGetCrewTimePercentageIncludesAbsoluteEndOfTheMonth()
        {
            var expectedDate = new DateTime(2011, 5, 1).AddTicks(-1);
            var expectedDateKey = new DateTime(2011, 4, 30);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new { Availability = 2.5m, Contractor = _currentUser.Contractor });
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder });

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.Count == 1, "There should only be one group returned");
            Assert.IsTrue(result.ContainsKey(expectedDateKey), "There should be a start-of-day DateTime value of the passed in expectedDate");
        }

        [TestMethod]
        public void TestGetCrewTimePercentageIncludesAbsoluteBeginningOfTheMonth()
        {
            var expectedDate = new DateTime(2011, 5, 1);
            var workOrder = CreateWorkOrderWithTimeToComplete(5m);
            var crew = GetFactory<CrewFactory>().Create(new { Availability = 2.5m, Contractor = _currentUser.Contractor });
            GetFactory<CrewAssignmentFactory>().Create(new { Crew = crew, AssignedFor = expectedDate, WorkOrder = workOrder });

            var result = Repository.GetCrewTimePercentagesByMonth(crew.Id, expectedDate);
            Assert.IsTrue(result.Count == 1, "There should only be one group returned, but " + result.Count + " were returned");
            Assert.IsTrue(result.ContainsKey(expectedDate), "There should be a start-of-day DateTime value of the passed in expectedDate");
        }


        #endregion

        #region GetAllForWorkOrderAssociatedContractor

        [TestMethod]
        public void TestGetAllForWorkOrderAssociatedContractorOnlyReturnsCrewAssignmentsForTheWorkOrdersCurrentAssignedContractor()
        {
            var goodCrew = GetFactory<CrewFactory>().Create(new { Contractor = _currentUser.Contractor });
            var badContractor = GetFactory<ContractorFactory>().Create();
            var badCrew = GetFactory<CrewFactory>().Create(new { Contractor = badContractor });
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {AssignedContractor = _currentUser.Contractor});
            var goodAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = goodCrew, AssignedFor = _testDate, WorkOrder = workOrder });
           
            // Needs to exist, don't need to check for it specifically.
            var badAss = GetFactory<CrewAssignmentFactory>().Create(new { Crew = badCrew, AssignedFor = _testDate, WorkOrder = workOrder });

            var all = Repository.GetAllForWorkOrderAssignedContractor(workOrder.Id).ToList();
            Assert.IsTrue(all.Count == 1);
            var resultGoodAss = all.First();
            Assert.AreEqual(goodAss.Id, resultGoodAss.Id);
        }
        

        #endregion

        #endregion
    }

    #region Test Repo

    public class TestCrewAssignmentRepository : CrewAssignmentRepository
    {
        public ICriteria CriteriaTest { get { return Criteria; } }
        public TestCrewAssignmentRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }
    }

    #endregion
}
