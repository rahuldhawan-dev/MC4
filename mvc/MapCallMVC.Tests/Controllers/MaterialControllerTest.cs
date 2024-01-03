using System.Collections.Generic;
using System.Linq;
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
    public class MaterialControllerTest : MapCallMvcControllerTestBase<MaterialController, Material>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var module = MaterialController.ROLE;
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Material/FindByPartialPartNumberOrDescription");
                a.RequiresRole("~/Material/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Material/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Material/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Material/New/", module, RoleActions.Add);
                a.RequiresRole("~/Material/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Material/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Material/Update/", module, RoleActions.Edit);
            });
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Material>().Create(new {PartNumber = "description 0"});
            var entity1 = GetEntityFactory<Material>().Create(new {PartNumber = "description 1"});
            var search = new SearchMaterial();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.PartNumber, "PartNumber");
                helper.AreEqual(entity1.PartNumber, "PartNumber", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Material>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditMaterial, Material>(eq, new {
                PartNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<Material>(eq.Id).PartNumber);
        }

        #endregion

        [TestMethod]
        public void TestByPartialPartNumberOrPartNumberReturnsMaterialByPartNumberOrPartNumber()
        {
            var material = GetEntityFactory<Material>().Create(new { Description = "Hydrant Thingy", PartNumber = "1800" });
            var invalid = GetEntityFactory<Material>().Create(new { Description = "Valve Thingy", PartNumber = "1700" });

            var result = (AutoCompleteResult)_target.FindByPartialPartNumberOrDescription("hyd");
            var model = (IEnumerable<dynamic>)result.Data;

            Assert.AreSame(material, model.Single());
            Assert.AreEqual(1, model.Count());

        }
    }
}
