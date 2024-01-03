using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPrePlanning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderPrePlanningControllerTest : MapCallMvcControllerTestBase<WorkOrderPrePlanningController, WorkOrder, WorkOrderRepository>
    {
        #region Fields

        private OperatingCenter _operatingCenter;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _authenticationService.SetupGet(x => x.CurrentUserIsAdmin).Returns(true);
            _operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // The auto test thing doesn't know how to create a WorkOrder entity
            // that works with the extra repository filtering for pre-planning atm.
            // options.RunShowReturnsViewForRecordTest = false;
            options.CreateValidEntity = () => {
                var routine = GetFactory<RoutineMarkoutRequirementFactory>().Create();
                return GetEntityFactory<WorkOrder>().Create(new {
                    MarkoutRequirement = routine,
                    OperatingCenter = _operatingCenter
                });
            };

            options.InitializeSearchTester = t =>
                t.TestPropertyValues[nameof(SearchWorkOrderPrePlanning.OperatingCenter)] = _operatingCenter.Id;
        }

        #endregion

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/WorkOrderPrePlanning/Search", module);
                a.RequiresRole("~/FieldOperations/WorkOrderPrePlanning/Show", module);
                a.RequiresRole("~/FieldOperations/WorkOrderPrePlanning/Index", module);
                a.RequiresRole("~/FieldOperations/WorkOrderPrePlanning/Assign", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrderPrePlanning/Update", module, RoleActions.Edit);
            });
        }

        #region Search

        [TestMethod]
        public void TestSearchSetsOperatingCenterDropDownData()
        {
            var expected = GetEntityFactory<OperatingCenter>().CreateList(3, new {
                WorkOrdersEnabled = true
            });
            _target.Search();

            _target.AssertHasDropDownData(expected, oc => oc.Id, oc => oc.ToString());
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexRedirectsBackToSearchWhenResultsAreOVERONETHOUSAND()
        {
            var mockRepo = new Mock<IWorkOrderRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<WorkOrderPrePlanningController>();
            mockRepo.Setup(r => r.GetPrePlanningWorkOrders(It.IsAny<SearchWorkOrderPrePlanning>()))
                    .Callback<ISearchSet<WorkOrder>>(action: s => { s.Count = 1001; })
                    .Returns(new WorkOrder[0]);

            var result = (RedirectToRouteResult)_target.Index(new SearchWorkOrderPrePlanning());

            Assert.AreEqual("Search", result.RouteValues["action"]);
            Assert.AreEqual("WorkOrderPrePlanning", result.RouteValues["controller"]);
        }

        #endregion

        #region Assign

        [TestMethod]
        public void TestAssignCreatesANewCrewAssignmentForEachWorkOrderId()
        {
            var now = DateTime.Now;
            _container.Inject<IDateTimeProvider>(new TestDateTimeProvider(now));
            var routine = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var orders = GetEntityFactory<WorkOrder>().CreateList(3, new {
                MarkoutRequirement = routine
            });
            var user = GetEntityFactory<User>().Create();

            _target.Assign(new AssignWorkOrderPrePlanning {
                WorkOrderIds = orders.Select(x => x.Id).ToArray(),
                AssignedTo = user.Id
            });

            foreach (var order in orders)
            {
                var persisted = Session.Load<WorkOrder>(order.Id);

                Assert.AreEqual(user.Id, persisted.OfficeAssignment.Id);
                Assert.AreEqual(now, persisted.OfficeAssignedOn);
            }
        }

        [TestMethod]
        public void TestUpdateRedirectsToSearchWhenNoWorkOrderIdsSelected()
        {
            var result = (RedirectToRouteResult)_target.Update(new UpdateWorkOrderPrePlanning(_container));

            Assert.AreEqual("Search", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void TestUpdateSetsPlannedCompletionDateAndSapErrorForSelectedWorkOrders()
        {
            var workOrders = GetEntityFactory<WorkOrder>().CreateList(5);
            var now = DateTime.Now;
            var model = new UpdateWorkOrderPrePlanning(_container) { WorkOrderIds = new[] {1, 3, 5}, PlannedCompletionDate = now };

            _target.Update(model);

            Session.Flush();
            Assert.AreEqual(now, workOrders[0].PlannedCompletionDate);
            Assert.AreEqual(WorkOrderPrePlanningController.RETRY_MESSAGE, workOrders[0].SAPErrorCode);
            Assert.IsNull(workOrders[1].PlannedCompletionDate);
            Assert.AreEqual(now, workOrders[2].PlannedCompletionDate);
            Assert.AreEqual(WorkOrderPrePlanningController.RETRY_MESSAGE, workOrders[2].SAPErrorCode);
            Assert.IsNull(workOrders[3].PlannedCompletionDate);
            Assert.AreEqual(now, workOrders[4].PlannedCompletionDate);
            Assert.AreEqual(WorkOrderPrePlanningController.RETRY_MESSAGE, workOrders[4].SAPErrorCode);
        }
        
        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            Assert.Inconclusive("This is handled in other tests");
            // noop this is handled in other tests
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("This is handled in other tests");
        }

        [TestMethod]
        public override void TestUpdateReturnsNotFoundIfRecordBeingUpdatedDoesNotExist()
        {
            Assert.Inconclusive("This is handled in other tests"); 
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            Assert.Inconclusive("This is handled in other tests");
        }

        #endregion

        #endregion
    }
}
