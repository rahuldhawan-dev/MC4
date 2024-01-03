using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class ChemicalControllerTest : MapCallMvcControllerTestBase<ChemicalController, Chemical>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/Chemical/Search/", role);
                a.RequiresRole("~/Environmental/Chemical/Show/", role);
                a.RequiresRole("~/Environmental/Chemical/Index/", role);
                a.RequiresRole("~/Environmental/Chemical/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/Chemical/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/Chemical/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/Chemical/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/Chemical/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Chemical>().Create(new {Name = "description 0"});
            var entity1 = GetEntityFactory<Chemical>().Create(new {Name = "description 1"});
            var search = new SearchChemical();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Name, "Name");
                helper.AreEqual(entity1.Name, "Name", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Chemical>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditChemical, Chemical>(eq, new {
                Name = expected
            }));

            Assert.AreEqual(expected, Session.Get<Chemical>(eq.Id).Name);
        }

        #endregion
    }
}
