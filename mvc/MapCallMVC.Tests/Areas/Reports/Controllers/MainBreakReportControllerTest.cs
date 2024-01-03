using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class MainBreakReportControllerTest : MapCallMvcControllerTestBase<MainBreakReportController, WorkOrder, WorkOrderRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/MainBreakReport/Search");
                a.RequiresLoggedInUserOnly("~/Reports/MainBreakReport/Index");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var eq1 = GetEntityFactory<WorkOrder>().Create();
            var eq2 = GetEntityFactory<WorkOrder>().Create();
            var search = new SearchMainBreak();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((IEnumerable<MainBreakReport>)result.Model).ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreEqual(0, resultModel[0].Total);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<WorkOrder>().Create();
            var entity1 = GetEntityFactory<WorkOrder>().Create();
            var search = new SearchMainBreak();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(0, "Total");
            }
        }

        #endregion
    }
}
