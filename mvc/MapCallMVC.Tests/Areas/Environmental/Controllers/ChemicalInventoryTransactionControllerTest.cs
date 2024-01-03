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
    public class ChemicalInventoryTransactionControllerTest : MapCallMvcControllerTestBase<ChemicalInventoryTransactionController, ChemicalInventoryTransaction>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalInventoryTransactionController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalInventoryTransaction/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalInventoryTransaction>().Create(new { InventoryRecordType = "description 0"});
            var entity1 = GetEntityFactory<ChemicalInventoryTransaction>().Create(new { InventoryRecordType = "description 1"});
            var search = new SearchChemicalInventoryTransaction();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.InventoryRecordType, "InventoryRecordType");
                helper.AreEqual(entity1.InventoryRecordType, "InventoryRecordType", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ChemicalInventoryTransaction>().Create();
            var expected = "some text";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalInventoryTransaction, ChemicalInventoryTransaction>(eq, new {
                InventoryRecordType = expected
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalInventoryTransaction>(eq.Id).InventoryRecordType);
        }

        #endregion
	}
}
