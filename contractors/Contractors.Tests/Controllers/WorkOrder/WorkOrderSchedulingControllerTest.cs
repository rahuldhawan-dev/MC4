using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderSchedulingControllerTest : ContractorControllerTestBase<WorkOrderSchedulingController, MapCall.Common.Model.Entities.WorkOrder>
    {
        #region Private Members

        private Mock<IRepository<Crew>> _mockCrewRepository;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            _mockCrewRepository = e.For<IRepository<Crew>>().Mock();
        }

        [TestCleanup]
        protected void TestCleanup()
        {
            _mockCrewRepository.VerifyAll();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () =>
                GetFactory<SchedulingWorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor,
                });
            options.InitializeSearchTester = (tester) =>
            {
                // No clue how this property is used for the search. It doesn't exist on WorkOrder.
                tester.IgnoredPropertyNames.Add(nameof(WorkOrderSchedulingSearch.CompletionTime));

                // This is a logical property on WorkOrder. Don't know how it's used for searching.
                tester.IgnoredPropertyNames.Add(nameof(WorkOrderSchedulingSearch.MarkoutRequired));

                // Don't even know what either of these properties are for.
                tester.IgnoredPropertyNames.Add(nameof(WorkOrderSchedulingSearch.MarkoutExpirationDays));

                // For reasons unknown to me, the default factories are not creating usable values in 
                // the contractors tests. So some of them need to be created with proper values manually.
                tester.TestPropertyValues[nameof(WorkOrderSchedulingSearch.Street)] = GetFactory<StreetFactory>().Create(new { Name = "Street Name" }).Id;

                // Tester needs one of the specific factories for Priority or else an error is thrown.
                tester.TestPropertyValues[nameof(WorkOrderGeneralSearch.Priority)] = GetFactory<RoutineWorkOrderPriorityFactory>().Create().Id;
            };
        }

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/WorkOrderScheduling/Index");
                a.RequiresSiteAdminUser("~/WorkOrderScheduling/Search");
            });
        }

        #endregion

        [TestMethod]
        public void TestSearchOnPropertiesThatSeeminglyDoNotWorkAndAreCurrentlyIgnored()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public void TestIndexRendersViewWithOrders()
        {
            // Leaving this rather than overriding the auto test due to extra checks.
            var expectedTown = GetFactory<TownFactory>().Create();
            var expected = GetFactory<SchedulingWorkOrderFactory>().CreateArray(2, new {
                AssignedContractor = _currentUser.Contractor, Town = expectedTown
            });

            // extra because of contractor
            GetFactory<SchedulingWorkOrderFactory>().CreateList(2, new {
                AssignedContractor = GetFactory<ContractorFactory>().Create(),
                Town = expectedTown
            });

            // extra because of town
            GetFactory<SchedulingWorkOrderFactory>().CreateList(2, new {
                AssignedContractor = _currentUser.Contractor,
                Town = GetFactory<TownFactory>().Create()
            });
            Session.Flush();
            var search = new WorkOrderSchedulingSearch {
                Town = expectedTown.Id
            };

            var result = (ViewResult)_target.Index(search);
            var model = (SchedulingCrewAssignment)result.Model;
            Assert.AreSame(search, model.Search);
            Assert.AreEqual(expected.Length, model.WorkOrders.Count());
            foreach (var order in model.WorkOrders)
            {
                Assert.IsTrue(expected.Contains(order));
            }
        }

        #region Search

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            // override needed because inherited Search action does not
            // return a model intance.
            var result = (ViewResult)_target.Search();
            MvcAssert.IsViewNamed(result, "Search");
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void TestIndexReturnsResultsWithPendingAssignment()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var wo1 = GetFactory<SchedulingWorkOrderFactory>().Create(new {
                OperatingCenter = opc,
                AssignedContractor = _currentUser.Contractor
            });
            var ca1 = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo1,
                AssignedFor = DateTime.Today
            });
            wo1.CrewAssignments.Add(ca1);

            var wo2 = GetFactory<SchedulingWorkOrderFactory>().Create(new {
                OperatingCenter = opc,
                AssignedContractor = _currentUser.Contractor
            });
            var ca2 = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo2,
                DateStarted = DateTime.Today.AddDays(-5),
                DateEnded = DateTime.Today.AddDays(-2),
                AssignedFor = DateTime.Today.AddDays(-1)
            });
            wo2.CrewAssignments.Add(ca2);
            Session.Evict(wo1);
            Session.Evict(wo2);

            var search = new WorkOrderSchedulingSearch { OperatingCenter = opc.Id };
            var result = (ViewResult)_target.Index(search);

            Assert.AreEqual(2, search.Count);

            search.HasPendingAssignments = true;
            _ = (ViewResult)_target.Index(search);

            Assert.AreEqual(1, search.Count);
            Assert.AreEqual(wo1.Id, search.Results.First().Id);

            search.HasPendingAssignments = false;
            result = (ViewResult)_target.Index(search);

            Assert.AreEqual(1, search.Count);
            Assert.AreEqual(wo2.Id, search.Results.First().Id);
        }

        #endregion

        #endregion
    }
}
