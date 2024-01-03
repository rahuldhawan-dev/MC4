using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class CompletedWorkOrdersWithMarkoutControllerTest : MapCallMvcControllerTestBase<CompletedWorkOrdersWithMarkoutController, WorkOrder, WorkOrderRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<WorkOrderFactory>().Create(new { DateCompleted = DateTime.Today });
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/CompletedWorkOrdersWithMarkout/Search", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/Reports/CompletedWorkOrdersWithMarkout/Index", role, RoleActions.UserAdministrator);
            });
        }

        [TestMethod]
        public void TestIndexSetsEnablePagingToFalseBecauseThereIsNoPagingEnabledForThisActionForAnything()
        {
            var search = new SearchCompletedWorkOrdersWithMarkout();
            search.EnablePaging = true;
            search.DateCompleted = new MMSINC.Data.RequiredDateRange()
            {
                Start = DateTime.Today,
                End = DateTime.Today
            };

            _target.Index(search);

            Assert.IsFalse(search.EnablePaging);
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            // None of these dates matter, it's just to get a single result for this test.
            var workorder = GetFactory<WorkOrderFactory>().Create(new { DateCompleted = DateTime.Today });
            var search = new SearchCompletedWorkOrdersWithMarkout();
            search.DateCompleted = new MMSINC.Data.RequiredDateRange()
            {
                Start = DateTime.Today,
                End = DateTime.Today
            };
            var result = (ViewResult)_target.Index(search);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, search.Results.Single().WorkOrderCount);
        }

        [TestMethod]
        public void TestIndexSupportsExcel()
        {
            var workorder = GetFactory<WorkOrderFactory>().Create(new { DateCompleted = DateTime.Today });
            var markout = GetFactory<MarkoutFactory>().Create(new { WorkOrder = workorder });
            var search = new SearchCompletedWorkOrdersWithMarkout();
            search.DateCompleted = new MMSINC.Data.RequiredDateRange()
            {
                Start = DateTime.Today,
                End = DateTime.Today
            };
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(workorder.OperatingCenter.OperatingCenterCode, "OperatingCenter");
                helper.AreEqual(workorder.Town.County.State, "State");
                helper.AreEqual(workorder.WorkDescription.Description, "WorkDescription");
                helper.AreEqual(1, "WorkOrderCount");
                helper.AreEqual(1, "MarkoutNoneCount");
                helper.AreEqual(0, "MarkoutRoutineCount");
                helper.AreEqual(0, "MarkoutEmergencyCount");
                helper.AreEqual(1, "PercentageNone");
                helper.AreEqual(0, "PercentageRoutine");
                helper.AreEqual(0, "PercentageEmergency");
            }
        }

    }
}
