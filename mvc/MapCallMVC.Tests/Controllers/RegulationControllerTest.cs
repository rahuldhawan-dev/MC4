using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class RegulationControllerTest : MapCallMvcControllerTestBase<RegulationController, Regulation>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RegulationController.ROLE;
                a.RequiresRole("~/Regulation/Search/", module);
                a.RequiresRole("~/Regulation/Show/", module);
                a.RequiresRole("~/Regulation/Index/", module);
                a.RequiresRole("~/Regulation/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Regulation/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Regulation/New/", module, RoleActions.Add);
                a.RequiresRole("~/Regulation/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Regulation/Destroy/", module, RoleActions.Delete);
            });
        }

        #endregion

        #region Index
     
        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Regulation>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<Regulation>().Create(new {Description = "description 1"});
            var search = new SearchRegulation();
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
            var eq = GetEntityFactory<Regulation>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRegulation, Regulation>(eq, new {
                RegulationShort = expected
            }));

            Assert.AreEqual(expected, Session.Get<Regulation>(eq.Id).RegulationShort);
        }

        #endregion
    }
}
