using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class OneCallMarkoutAuditControllerTest : MapCallMvcControllerTestBase<OneCallMarkoutAuditController, OneCallMarkoutAudit>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = OneCallMarkoutAuditController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/OneCallMarkoutAudit/Search/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutAudit/Show/", role);
                a.RequiresRole("~/FieldOperations/OneCallMarkoutAudit/Index/", role);
            });
        }				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<OneCallMarkoutAudit>().Create(new { FullText = "description 0"});
            var entity1 = GetEntityFactory<OneCallMarkoutAudit>().Create(new { FullText = "description 1" });
            var search = new SearchOneCallMarkoutAudit();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion
    }
}
