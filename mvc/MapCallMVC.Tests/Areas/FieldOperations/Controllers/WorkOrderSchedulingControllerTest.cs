using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduling;
using MapCall.Common.Testing.Data;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderSchedulingControllerTest : MapCallMvcControllerTestBase<WorkOrderSchedulingController, WorkOrder, WorkOrderRepository>
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
            AddWorkManagementRoleToCurrentUserForOperatingCenter(_operatingCenter);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var opc = GetEntityFactory<OperatingCenter>().Create();
                var wo = GetEntityFactory<WorkOrder>().Create(new {
                    OperatingCenter = opc
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(opc);
                return wo;
            };
        }

        #endregion

        #region Private Methods

        private void AddWorkManagementRoleToCurrentUserForOperatingCenter(OperatingCenter opc)
        {
            var role = GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, opc, _currentUser, RoleActions.UserAdministrator);
        }        

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/WorkOrderScheduling/Search", module);
                a.RequiresRole("~/FieldOperations/WorkOrderScheduling/Index", module);
                a.RequiresRole("~/FieldOperations/WorkOrderScheduling/Show", module);
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduling/CanBeScheduled/");
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

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // there is no regular show method, no need to test
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opc);
            var wo1 = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = GetFactory<EmergencyMarkoutRequirementFactory>().Create(),
                OperatingCenter = opc
            });
            var ca1 = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo1,
                AssignedFor = _now.Date
            });
            wo1.CrewAssignments.Add(ca1);
            Session.Evict(wo1);

            InitializeControllerAndRequest("~/FieldOperations/WorkOrderScheduling/Show/" + wo1.Id + ".frag");

            var result = _target.Show(wo1.Id);
            MvcAssert.IsViewNamed(result, "_ShowPopup");
        }

        #endregion
        #region Index

        [TestMethod]
        public void TestIndexRedirectsBackToSearchWhenResultsAreOVERONETHOUSAND()
        {
            var mockRepo = new Mock<IWorkOrderRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<WorkOrderSchedulingController>();
            mockRepo.Setup(r => r.Search(It.IsAny<SearchWorkOrderScheduling>()))
                    .Callback<ISearchSet<WorkOrder>>(action: s => { s.Count = 1001; })
                    .Returns(new WorkOrder[0]);

            var result = (RedirectToRouteResult)_target.Index(new SearchWorkOrderScheduling());

            Assert.AreEqual("Search", result.RouteValues["action"]);
            Assert.AreEqual("WorkOrderScheduling", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestIndexReturnsResultsWithPendingAssignment()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opc);
            var wo1 = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = GetFactory<EmergencyMarkoutRequirementFactory>().Create(),
                OperatingCenter = opc
            });
            var ca1 = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo1,
                AssignedFor = _now.Date
            });
            wo1.CrewAssignments.Add(ca1);

            var wo2 = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = GetFactory<EmergencyMarkoutRequirementFactory>().Create(),
                OperatingCenter = opc
            });
            var ca2 = GetFactory<CrewAssignmentFactory>().Create(new {
                WorkOrder = wo2,
                DateStarted = _now.Date.AddDays(-5),
                DateEnded = _now.Date.AddDays(-2),
                AssignedFor = _now.Date.AddDays(-1)
            });
            wo2.CrewAssignments.Add(ca2);
            Session.Evict(wo1);
            Session.Evict(wo2);

            var search = new SearchWorkOrderScheduling { OperatingCenter = opc.Id };
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

        [TestMethod]
        public void TestIndexReturnsResultsWithMultiplePurposeSelected()
        {
            var purpose1 = GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            var purpose2 = GetFactory<ComplianceWorkOrderPurposeFactory>().Create();
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var wo1 = GetFactory<SchedulingWorkOrderFactory>().Create(new { OperatingCenter = opc, Purpose = purpose1 });
            var wo2 = GetFactory<SchedulingWorkOrderFactory>().Create(new { OperatingCenter = opc, Purpose = purpose2 });
            var wo3 = GetFactory<SchedulingWorkOrderFactory>().Create(new { OperatingCenter = opc, Purpose = purpose1 });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opc);
            var search = new SearchWorkOrderScheduling {
                OperatingCenter = opc.Id,
                Purpose = new [] { purpose1.Id }
            };

            var result = _target.Index(search);

            Assert.AreEqual(2, search.Count);
            Assert.IsTrue(search.Results.Any(x => x.Id == wo1.Id));
            Assert.IsTrue(search.Results.Any(x => x.Id == wo3.Id));

            search.Purpose = new [] { purpose1.Id, purpose2.Id };
            result = _target.Index(search);

            Assert.AreEqual(3, search.Count);
            Assert.IsTrue(search.Results.Any(x => x.Id == wo1.Id));
            Assert.IsTrue(search.Results.Any(x => x.Id == wo3.Id));
            Assert.IsTrue(search.Results.Any(x => x.Id == wo3.Id));
        }

        [TestMethod]
        public void TestIndexRespondsToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/WorkOrderScheduling/Index.map");
            var search = new SearchWorkOrderScheduling();
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        #endregion

        #region CanBeScheduled

        [TestMethod]
        public void Test_CanBeScheduled_ReturnsFalse_ForWorkOrderWithInvalidMarkout()
        {
            var markoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = _operatingCenter
            });
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(-2),
                ExpirationDate = _now.AddDays(5),
                WorkOrder = workOrder
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now.AddDays(7)) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;
            Assert.IsFalse(data.CanBeScheduled);
        }

        [TestMethod]
        public void Test_CanBeScheduled_ReturnsFalse_ForWorkOrderWithInvalidPermit()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                StreetOpeningPermitRequired = true,
                OperatingCenter = _operatingCenter
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now.AddDays(7)) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;
            Assert.IsFalse(data.CanBeScheduled);
        }

        [TestMethod]
        public void Test_CanBeScheduled_ReturnsTrue_ForWorkOrderWithNoPermitOrMarkoutRequirements()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now.AddDays(7)) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;

            Assert.IsTrue(data.CanBeScheduled);
        }

        [TestMethod]
        public void Test_CanBeScheduled_ReturnsTrue_WhenScheduledForFutureWithFutureAndExpiredMarkout()
        {
            var markoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = _operatingCenter
            });
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(-12),
                ExpirationDate = _now.AddDays(-3),
                WorkOrder = workOrder
            });
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(3),
                ExpirationDate = _now.AddDays(11),
                WorkOrder = workOrder
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now.AddDays(7)) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;
            Assert.IsTrue(data.CanBeScheduled);
        }

        [TestMethod]
        public void Test_CanBeScheduled_ReturnsTrue_WhenScheduledForFutureWithFutureAndValidMarkout()
        {
            var markoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = _operatingCenter
            });
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(-12),
                ExpirationDate = _now.AddDays(-3),
                WorkOrder = workOrder
            });
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(-2),
                ExpirationDate = _now.AddDays(5),
                WorkOrder = workOrder
            });
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(4),
                ExpirationDate = _now.AddDays(11),
                WorkOrder = workOrder
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now.AddDays(7)) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;
            Assert.IsTrue(data.CanBeScheduled);
        }

        [TestMethod]
        public void
            Test_CanBeScheduled_ReturnsFalse_WhenMarkoutExpirationDateDayIsSameAsScheduledForDayAndOrderIsForNJ()
        {
            _now = new DateTime(_now.Year, _now.Month, _now.Day, 12, 0, 0);
            _container.Inject<IDateTimeProvider>(new TestDateTimeProvider(_now));

            var nj = GetEntityFactory<State>().Create(new {Abbreviation = "NJ"});
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = nj });
            var markoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = operatingCenter
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(-12),
                ExpirationDate = _now.AddHours(1),
                WorkOrder = workOrder
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;
            Assert.IsFalse(data.CanBeScheduled);
        }

        [TestMethod]
        public void
            Test_CanBeScheduled_ReturnsTrue_WhenMarkoutExpirationDateDayIsSameAsScheduledForDayAndOrderIsNotForNJ()
        {
            _now = new DateTime(_now.Year, _now.Month, _now.Day, 12, 0, 0);
            _container.Inject<IDateTimeProvider>(new TestDateTimeProvider(_now));

            var ny = GetEntityFactory<State>().Create(new {Abbreviation = "NY"});
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new {
                OperatingCenterCode = "NY1",
                State = ny
            });
            var markoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = operatingCenter
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            GetEntityFactory<Markout>().Create(new {
                ReadyDate = _now.AddDays(-12),
                ExpirationDate = _now.AddHours(1),
                WorkOrder = workOrder
            });

            var result = _target.CanBeScheduled(workOrder.Id, _now) as JsonResult;
            var data = (CanBeScheduledResult)result.Data;
            Assert.IsTrue(data.CanBeScheduled);
        }

        #endregion

        #endregion
    }
}
