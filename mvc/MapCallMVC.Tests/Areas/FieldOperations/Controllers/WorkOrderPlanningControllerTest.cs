using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPlanning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderPlanningControllerTest
        : MapCallMvcControllerTestBase<WorkOrderPlanningController, WorkOrder, WorkOrderRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.CreateValidEntity = () => {
                var wo = GetEntityFactory<WorkOrder>().Create(new {
                    MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                    StreetOpeningPermitRequired = true
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
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

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesWorkManagement;
            var baseUrl = "~/FieldOperations/WorkOrderPlanning/";
            Authorization.Assert(a => {
                a.RequiresRole(baseUrl + "Search", module);
                a.RequiresRole(baseUrl + "Show", module);
                a.RequiresRole(baseUrl + "Index", module);
                a.RequiresRole(baseUrl + "Update", module, RoleActions.Edit);
                a.RequiresRole(baseUrl + "Edit", module, RoleActions.Edit);
                a.RequiresRole(baseUrl + "UpdatePlanningForTrafficControl", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // noop
        }

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            //noop
        }

        [TestMethod]
        public void TestEditReturnsEmptyResult()
        {
            var result = (EmptyResult)_target.Edit(0);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestUpdatePlanningForTrafficControlRedirectsBackToTheReferrerIfSet()
        {
            var entity = GetFactory<WorkOrderFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));
            
            var result = (RedirectResult)_target.UpdatePlanningForTrafficControl(new UpdateWorkOrderPlanning(_container) {
                Id = entity.Id
            });
            
            Assert.AreEqual(url + WorkOrderPlanningController.TRAFFIC_CONTROL_FRAGMENT_IDENTIFIER, result.Url);
        }

        #endregion

        #region Show/Index

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetFactory<WorkOrderFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(good.OperatingCenter);
            var bad = GetFactory<WorkOrderFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(bad.OperatingCenter);
            InitializeControllerAndRequest("~/FieldOperations/WorkOrderPlanning/Show/" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var good = GetFactory<WorkOrderFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(good.OperatingCenter);

            InitializeControllerAndRequest("~/FieldOperations/WorkOrderPlanning/Show/" + good.Id + ".frag");

            var result = _target.Show(good.Id);
            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, good);
        }
        
        [TestMethod]
        public void TestIndexRespondsToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/WorkOrderPlanning/Index.map");
            var search = new SearchWorkOrderPlanning(_dateTimeProvider);
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(MapResult));
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
        public void TestUpdateRedirectsToSearchWhenNoWorkOrderIdsSelected()
        {
            var result = (RedirectToRouteResult)_target.Update(new UpdateWorkOrderPlanning(_container));

            Assert.AreEqual("Search", result.RouteValues["Action"]);
        }

        #endregion
    }
}
