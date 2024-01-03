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
    public class WaterConstituentControllerTest : MapCallMvcControllerTestBase<WaterConstituentController, WaterConstituent>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = WaterConstituentController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/WaterConstituent/Search/", role);
                a.RequiresRole("~/WaterQuality/WaterConstituent/Show/", role);
                a.RequiresRole("~/WaterQuality/WaterConstituent/Index/", role);
                a.RequiresRole("~/WaterQuality/WaterConstituent/New/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/WaterConstituent/Create/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/WaterConstituent/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/WaterConstituent/Update/", role, RoleActions.Edit);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<WaterConstituent>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<WaterConstituent>().Create(new {Description = "description 1"});
            var search = new SearchWaterConstituent();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WaterConstituent>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWaterConstituent, WaterConstituent>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<WaterConstituent>(eq.Id).Description);
        }

        #endregion
	}
}
