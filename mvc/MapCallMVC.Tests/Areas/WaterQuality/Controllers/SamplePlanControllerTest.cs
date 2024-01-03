using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class SamplePlanControllerTest : MapCallMvcControllerTestBase<SamplePlanController, SamplePlan>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = SamplePlanController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/SamplePlan/Search/", role);
                a.RequiresRole("~/WaterQuality/SamplePlan/Show/", role);
                a.RequiresRole("~/WaterQuality/SamplePlan/Index/", role);
                a.RequiresRole("~/WaterQuality/SamplePlan/New/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/SamplePlan/Create/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/SamplePlan/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SamplePlan/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SamplePlan/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SamplePlan>().Create();
            var entity1 = GetEntityFactory<SamplePlan>().Create();
            var search = new SearchSamplePlan();
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
