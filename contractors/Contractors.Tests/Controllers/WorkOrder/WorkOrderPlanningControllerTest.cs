using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderPlanningControllerTest : ContractorControllerTestBase<WorkOrderPlanningController, MapCall.Common.Model.Entities.WorkOrder>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<PlanningWorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            options.InitializeSearchTester = (tester) =>
            {
                // For reasons unknown to me, the default factories are not creating usable values in 
                // the contractors tests. So some of them need to be created with proper values manually.
                tester.TestPropertyValues[nameof(WorkOrderPlanningSearch.Street)] = GetFactory<StreetFactory>().Create(new { Name = "Street Name" }).Id;

                // Tester needs one of the specific factories for Priority or else an error is thrown.
                tester.TestPropertyValues[nameof(WorkOrderGeneralSearch.Priority)] = GetFactory<RoutineWorkOrderPriorityFactory>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/WorkOrderPlanning/Search");
                a.RequiresSiteAdminUser("~/WorkOrderPlanning/Index");
                a.RequiresSiteAdminUser("~/WorkOrderPlanning/Show");
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexRendersViewWithOrders()
        {
            // Leaving this rather than overriding the auto test due to extra checks.
            var expectedTown = GetFactory<TownFactory>().Create();
            var expected = GetFactory<PlanningWorkOrderFactory>().CreateArray(2, new {
                AssignedContractor = _currentUser.Contractor, Town = expectedTown
            });

            // extra because of contractor
            GetFactory<PlanningWorkOrderFactory>().CreateList(2, new {
                AssignedContractor = GetFactory<ContractorFactory>().Create(),
                Town = expectedTown
            });

            // extra because of town
            GetFactory<PlanningWorkOrderFactory>().CreateList(2, new {
                AssignedContractor = _currentUser.Contractor,
                Town = GetFactory<TownFactory>().Create()
            });

            var search = new WorkOrderPlanningSearch {
                Town = expectedTown.Id
            };

            var result = (ViewResult)_target.Index(search);
            Assert.AreSame(search, result.Model);
            // note the page size is involved here.
            Assert.AreEqual(expected.Length, search.Results.Count());
            foreach (var order in search.Results)
            {
                Assert.IsTrue(expected.Contains(order));
            }
        }

        #endregion

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

        #endregion

        #region Show

        [TestMethod]
        public void TestShowReturnsNotFoundIfNotInPlanning()
        {
            var expected = GetFactory<FinalizationWorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor,
            });
            Session.Flush();
            MvcAssert.IsStatusCode(404, _target.Show(expected.Id));
        }

        #endregion
    }
}
