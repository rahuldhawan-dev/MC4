using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class ChemicalStorageControllerTest : MapCallApiControllerTestBase<ChemicalStorageController, ChemicalStorage, IRepository<ChemicalStorage>>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.EnvironmentalGeneral;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/ChemicalStorage", module);
            });
        }

        [TestMethod]
        // Adding this override as this test is running but is not applicable for this controller
        public override void TestIndexReturnsResults() { }

        [TestMethod]
        public void Test_Index_ReturnsProperJson()
        {
            var state = GetEntityFactory<State>().Create();
            var town = GetEntityFactory<Town>().Create(new {State = state});
            var chemical = GetEntityFactory<Chemical>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {PublicWaterSupply = publicWaterSupply, OperatingCenter = operatingCenter, Town = town});
            var warehouseNumber = GetEntityFactory<ChemicalWarehouseNumber>().Create();
            var chemicalStorage = GetEntityFactory<ChemicalStorage>()
                .CreateList(3, new { Facility = facility, WarehouseNumber = warehouseNumber, Chemical = chemical, IsActive = true });
            
            var searchModel = new SearchChemicalStorage
                { Facility = facility.Id, PublicWaterSupply = publicWaterSupply.Id, State = state.Id };
            
            var result = (JsonResult)_target.Index(searchModel);
            var helper = new JsonResultTester(result);

            Assert.AreEqual(helper.Count, chemicalStorage.Count);
            
            for (var i = 0; i < chemicalStorage.Count; i++)
            {
                var currentChemicalStorage = chemicalStorage[i];
                helper.AreEqual(warehouseNumber.WarehouseNumber, nameof(warehouseNumber.WarehouseNumber), i);
                helper.AreEqual(facility.FacilityName, nameof(Facility), i);
                helper.AreEqual(chemical.Name, nameof(Chemical), i);
                helper.AreEqual(chemical.SdsHyperlink, nameof(chemical.SdsHyperlink), i);
                helper.AreEqual(chemical.PartNumber, nameof(chemical.PartNumber), i);
                helper.AreEqual(currentChemicalStorage.IsActive, nameof(currentChemicalStorage.IsActive), i);
                helper.AreEqual(currentChemicalStorage.Id, nameof(currentChemicalStorage.Id), i);
                helper.AreEqual(currentChemicalStorage.Facility.Town.State.Abbreviation, nameof(facility.Town.State), i);
                helper.AreEqual(currentChemicalStorage.MaxStorageQuantityGallons, nameof(currentChemicalStorage.MaxStorageQuantityGallons), i);
                helper.AreEqual(currentChemicalStorage.MinStorageQuantityGallons, nameof(currentChemicalStorage.MinStorageQuantityGallons), i);
                helper.AreEqual(currentChemicalStorage.MaxStorageQuantityPounds, nameof(currentChemicalStorage.MaxStorageQuantityPounds), i);
                helper.AreEqual(currentChemicalStorage.MinStorageQuantityPounds, nameof(currentChemicalStorage.MinStorageQuantityPounds), i);
            }
        }
    }
}
