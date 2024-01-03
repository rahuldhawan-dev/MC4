using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderGeneralControllerTest : ContractorControllerTestBase<WorkOrderGeneralController, MapCall.Common.Model.Entities.WorkOrder>
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
            options.CreateValidEntity = () => GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            options.InitializeSearchTester = (tester) =>
            {
                // For reasons unknown to me, the default factories are not creating usable values in 
                // the contractors tests. So some of them need to be created with proper values manually.
                tester.TestPropertyValues[nameof(WorkOrderGeneralSearch.Street)] = GetFactory<StreetFactory>().Create(new { Name = "Street Name" }).Id;

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
                a.RequiresLoggedInUserOnly("~/WorkOrderGeneral/Index");
                a.RequiresLoggedInUserOnly("~/WorkOrderGeneral/Search");
                a.RequiresLoggedInUserOnly("~/WorkOrderGeneral/Show");
            });
        }

        #endregion

        #region Search

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            // override needed due to inherited Search action. The action
            // does not return a model instance.
            var result = (ViewResult)_target.Search();
            MvcAssert.IsViewNamed(result, "Search");
            Assert.IsNull(result.Model);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexRendersViewWithOrders()
        {
            // Leaving this test rather than overriding the automatic test due to the extra checks.
            var expectedTown = GetFactory<TownFactory>().Create();
            var expected = GetFactory<PlanningWorkOrderFactory>().CreateArray(2, new
            {
                AssignedContractor = _currentUser.Contractor,
                Town = expectedTown
            });

            // extra because of contractor
            GetFactory<PlanningWorkOrderFactory>().CreateList(2, new
            {
                AssignedContractor = GetFactory<ContractorFactory>().Create(),
                Town = expectedTown
            });

            // extra because of town
            GetFactory<PlanningWorkOrderFactory>().CreateList(2, new
            {
                AssignedContractor = _currentUser.Contractor,
                Town = GetFactory<TownFactory>().Create()
            });

            var search = new WorkOrderGeneralSearch
            {
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

        #region Show

        [TestMethod]
        public void TestShowReturnsViewWithCancellationError()
        {
            var expectedTown = GetFactory<TownFactory>().Create();
            var expected = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor,
                Town = expectedTown, 
                CancelledAt = DateTime.Now
            });
            expected.WorkOrderCancellationReason = new WorkOrderCancellationReason { Description = "Bees"};
            

            var result = _target.Show(expected.Id) as ViewResult;

            Assert.AreEqual($"Cancelled On: {expected.CancelledAt}, Reason: { expected.WorkOrderCancellationReason}", ((List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]).Single());
        }

        #endregion
    }
}