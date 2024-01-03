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
    public class ChemicalStorageControllerTest : MapCallMvcControllerTestBase<ChemicalStorageController, ChemicalStorage>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalStorageController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalStorage/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalStorage/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalStorage/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalStorage/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalStorage/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalStorage/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalStorage/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalStorage/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/Environmental/ChemicalStorage/ByChemical/", role);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalStorage>().Create(new {DeliveryInstructions = "description 0"});
            var entity1 = GetEntityFactory<ChemicalStorage>().Create(new {DeliveryInstructions = "description 1"});
            var search = new SearchChemicalStorage();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.DeliveryInstructions, "DeliveryInstructions");
                helper.AreEqual(entity1.DeliveryInstructions, "DeliveryInstructions", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSetsFacilitiesChemicalFeedToTrueWhenChemicalStorageIsCreatedForAFacility()
        {
            var state = GetEntityFactory<State>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter, PublicWaterSupply = publicWaterSupply, ChemicalFeed = false });
            var chemicalStorage = GetEntityFactory<ChemicalStorage>().Create(new { Facility = facility });

            _target.Create(_viewModelFactory.BuildWithOverrides<CreateChemicalStorage, ChemicalStorage>(chemicalStorage, new {
                Facility = facility.Id
            }));

            Repository.Load(chemicalStorage.Id);
            Assert.IsTrue(facility.ChemicalFeed);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ChemicalStorage>().Create();
            var expected = "description field";

            _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalStorage, ChemicalStorage>(eq, new {
                DeliveryInstructions = expected
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalStorage>(eq.Id).DeliveryInstructions);
        }

        [TestMethod]
        public void TestUpdateSetsFacilitiesChemicalFeedToTrueWhenChemicalStorageIsCreatedForAFacility()
        {
            var state = GetEntityFactory<State>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter, PublicWaterSupply = publicWaterSupply, ChemicalFeed = false });
            var chemical = GetEntityFactory<Chemical>().Create( new { Id = 9, Name = "Liquid Oxygen,BULK" });
            var replacementChemical = GetEntityFactory<Chemical>().Create(new { Id =11, Name = "Algicide" });
            var chemicalStorage = GetEntityFactory<ChemicalStorage>().Create(new { Facility = facility, Chemical = chemical });

            _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalStorage, ChemicalStorage>(chemicalStorage, new {
                Facility = facility.Id, Chemical = replacementChemical.Id
            }));

            Repository.Load(chemicalStorage.Id);
            Assert.IsTrue(facility.ChemicalFeed);
        }

        #endregion

        #region ByChemical

        [TestMethod]
        public void TestByChemicalReturnsChemicalStorageRecordsFilteredByChemical()
        {
            var goodChem = GetEntityFactory<Chemical>().Create();
            var badChem = GetEntityFactory<Chemical>().Create();
            var goodCs = GetEntityFactory<ChemicalStorage>().Create(new { Chemical = goodChem});
            GetEntityFactory<ChemicalStorage>().Create(new { Chemical = badChem});

            var result = (CascadingActionResult)_target.ByChemical(goodChem.Id);
            var data = (IEnumerable<ChemicalStorageDisplayItem>)result.Data;

            Assert.AreEqual(goodCs.Id, data.Single().Id);
        }

        #endregion
    }
}
