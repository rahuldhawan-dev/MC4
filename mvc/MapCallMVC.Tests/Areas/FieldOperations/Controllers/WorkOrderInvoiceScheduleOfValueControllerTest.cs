using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderInvoiceScheduleOfValueControllerTest : MapCallMvcControllerTestBase<WorkOrderInvoiceScheduleOfValueController, WorkOrderInvoiceScheduleOfValue>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditWorkOrderInvoiceScheduleOfValue)vm;
                return new { action = "Show", controller = "WorkOrderInvoice", area = "FieldOperations", id = model.WorkOrderInvoice };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/WorkOrderInvoiceScheduleOfValue/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoiceScheduleOfValue/Update/", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var scheduleOfValue = GetEntityFactory<ScheduleOfValue>().Create();
            var workOrderInvoice = GetEntityFactory<WorkOrderInvoice>().Create();
            var eq = GetEntityFactory<WorkOrderInvoiceScheduleOfValue>().Create(new { WorkOrderInvoice = workOrderInvoice, ScheduleOfValue = scheduleOfValue });
            decimal? expected = 4;
            var viewModel = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoiceScheduleOfValue, WorkOrderInvoiceScheduleOfValue>(eq, new {
                Total = expected
            });

            var result = _target.Update(viewModel);

            Assert.AreEqual(expected, Session.Get<WorkOrderInvoiceScheduleOfValue>(eq.Id).Total);
        }

        #endregion
    }
}