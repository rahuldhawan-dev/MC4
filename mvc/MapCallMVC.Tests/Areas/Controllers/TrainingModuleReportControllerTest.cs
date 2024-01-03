using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class TrainingModuleReportControllerTest : MapCallMvcControllerTestBase<TrainingModuleReportController, TrainingModule>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.OperationsTrainingModules;
                a.RequiresRole("~/Reports/TrainingModuleReport/Index", module);
            });
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TrainingModule>().Create(new { IsActive = true });
            var entity1 = GetEntityFactory<TrainingModule>().Create(new { IsActive = false });
            var search = new SearchTrainingModule();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "TrainingModuleID");
                helper.AreEqual(entity1.Id, "TrainingModuleID", 1);
                helper.AreEqual(entity0.IsActive, "IsActive");
                helper.AreEqual(entity1.IsActive, "IsActive", 1);
            }
        }
    }
}
