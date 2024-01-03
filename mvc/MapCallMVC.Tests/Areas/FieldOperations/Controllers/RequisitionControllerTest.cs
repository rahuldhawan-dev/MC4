using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class RequisitionControllerTest : MapCallMvcControllerTestBase<RequisitionController, Requisition>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.NewReturnsPartialView = true;
            options.ExpectedNewViewName = "_New";
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
            options.ExpectedShowViewName = "_ShowAjaxTableRow";
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
        }

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/Requisition/Show/", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/Requisition/New/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Requisition/Create/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Requisition/Edit/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Requisition/Update/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Requisition/Destroy/", role, RoleActions.UserAdministrator);
            });
        }

        #endregion

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override needed because of param on New action.
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            var result = (PartialViewResult)_target.New(workOrder.Id);
            MvcAssert.IsViewNamed(result, "_New");
            var model = (CreateWorkOrderRequisition)result.Model;
            Assert.AreEqual(workOrder.Id, model.WorkOrder);
        }

        #endregion
    }
}
