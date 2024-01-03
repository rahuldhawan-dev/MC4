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
    public class SampleIdMatrixControllerTest : MapCallMvcControllerTestBase<SampleIdMatrixController, SampleIdMatrix>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = SampleIdMatrixController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Search/", role);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Show/", role);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Index/", role);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/BySampleSiteId/", role);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/New/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Create/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/SampleIdMatrix/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SampleIdMatrix>().Create(new {Parameter = "description 0"});
            var entity1 = GetEntityFactory<SampleIdMatrix>().Create(new {Parameter = "description 1"});
            var search = new SearchSampleIdMatrix();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Parameter, "Parameter");
                helper.AreEqual(entity1.Parameter, "Parameter", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SampleIdMatrix>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSampleIdMatrix, SampleIdMatrix>(eq, new {
                Parameter = expected
            }));

            Assert.AreEqual(expected, Session.Get<SampleIdMatrix>(eq.Id).Parameter);
        }

        #endregion
	}
}
