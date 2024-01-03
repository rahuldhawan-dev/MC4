using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class ChemicalWarehouseNumberControllerTest : MapCallMvcControllerTestBase<ChemicalWarehouseNumberController, ChemicalWarehouseNumber>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalWarehouseNumberController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/Environmental/ChemicalWarehouseNumber/ByOperatingCenter/", role);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalWarehouseNumber>().Create(new {WarehouseNumber = "description 0"});
            var entity1 = GetEntityFactory<ChemicalWarehouseNumber>().Create(new {WarehouseNumber = "description 1"});
            var search = new SearchChemicalWarehouseNumber();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.WarehouseNumber, "WarehouseNumber");
                helper.AreEqual(entity1.WarehouseNumber, "WarehouseNumber", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestUpdateUpdatesChemicalWarehouseNumberAndRedirectsBackToShow()
        {
            var eq = GetEntityFactory<ChemicalWarehouseNumber>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalWarehouseNumber, ChemicalWarehouseNumber>(eq, new {
                WarehouseNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalWarehouseNumber>(eq.Id).WarehouseNumber);
        }

        #endregion

        #region ByOperatingCenter

        [TestMethod]
        public void TestByOperatingCenterReturnsChemicalWarehouseNumberRecordsFilteredByOperatingCenter()
        {
            var goodOpc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var badOpc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodCwn = GetEntityFactory<ChemicalWarehouseNumber>().Create(new { OperatingCenter = goodOpc });
            var badCwn = GetEntityFactory<ChemicalWarehouseNumber>().Create(new { OperatingCenter = badOpc });

            var result = (CascadingActionResult)_target.ByOperatingCenter(goodOpc.Id);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(goodCwn.Id, data.Single().Id);
        }

        #endregion
    }
}
