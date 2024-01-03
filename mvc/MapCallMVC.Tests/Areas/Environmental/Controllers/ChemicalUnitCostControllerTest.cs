using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class ChemicalUnitCostControllerTest : MapCallMvcControllerTestBase<ChemicalUnitCostController, ChemicalUnitCost>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalUnitCostController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/Environmental/ChemicalUnitCost/ByChemical/", role);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalUnitCost>().Create(new {ChemicalOrderingProcess = "description 0"});
            var entity1 = GetEntityFactory<ChemicalUnitCost>().Create(new {ChemicalOrderingProcess = "description 1"});
            var search = new SearchChemicalUnitCost();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ChemicalOrderingProcess, "ChemicalOrderingProcess");
                helper.AreEqual(entity1.ChemicalOrderingProcess, "ChemicalOrderingProcess", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ChemicalUnitCost>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalUnitCost, ChemicalUnitCost>(eq, new {
                ChemicalOrderingProcess = expected
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalUnitCost>(eq.Id).ChemicalOrderingProcess);
        }

        #endregion

        #region ByChemical

        [TestMethod]
        public void TestByChemicalReturnsChemicalUnitCostRecordsFilteredByChemical()
        {
            var goodChem = GetEntityFactory<Chemical>().Create();
            var badChem = GetEntityFactory<Chemical>().Create();
            var goodCs = GetEntityFactory<ChemicalUnitCost>().Create(new { Chemical = goodChem });
            var badCs = GetEntityFactory<ChemicalUnitCost>().Create(new { Chemical = badChem });

            var result = (CascadingActionResult)_target.ByChemical(goodChem.Id);
            var data = (IEnumerable<ChemicalUnitCostDisplayItem>)result.Data;

            Assert.AreEqual(goodCs.Id, data.Single().Id);
        }

        #endregion
    }
}
