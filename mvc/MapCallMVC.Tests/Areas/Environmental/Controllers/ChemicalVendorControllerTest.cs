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
    public class ChemicalVendorControllerTest : MapCallMvcControllerTestBase<ChemicalVendorController, ChemicalVendor>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalVendorController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalVendor/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalVendor/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalVendor/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalVendor/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalVendor/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalVendor/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalVendor/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalVendor/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalVendor>().Create(new {Vendor = "description 0"});
            var entity1 = GetEntityFactory<ChemicalVendor>().Create(new {Vendor = "description 1"});
            var search = new SearchChemicalVendor();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Vendor, "Vendor");
                helper.AreEqual(entity1.Vendor, "Vendor", 1);
            }
        }

        #endregion
        
        #region Edit/Update

        [TestMethod]
        public void TestUpdateUpdatesChemicalVendorAndRedirectsBackToShow()
        {
            var eq = GetEntityFactory<ChemicalVendor>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalVendor, ChemicalVendor>(eq, new {
                Vendor = expected
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalVendor>(eq.Id).Vendor);
        }

        #endregion
	}
}
