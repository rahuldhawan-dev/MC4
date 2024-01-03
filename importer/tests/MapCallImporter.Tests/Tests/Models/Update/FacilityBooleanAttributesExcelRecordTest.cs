using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Update;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Update
{
    [TestClass]
    public class FacilityBooleanAttributesExcelRecordTest : ExcelRecordTestBase<Facility, MyEditFacility, FacilityBooleanAttributesExcelRecord>
    {
        public const int FACILITY_ID = 11;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateSomeFacilitiesInAberdeenNJ(_container);
        }

        #endregion

        protected override FacilityBooleanAttributesExcelRecord CreateTarget()
        {
            return new FacilityBooleanAttributesExcelRecord { Id = FACILITY_ID };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Facility, MyEditFacility, FacilityBooleanAttributesExcelRecord> test)
        {
            test.Boolean(x => x.PropertyOnly, x => x.PropertyOnly);
            test.Boolean(x => x.Administration, x => x.Administration);
            test.Boolean(x => x.EmergencyPower, x => x.EmergencyPower);
            test.Boolean(x => x.GroundWaterSupply, x => x.GroundWaterSupply);
            test.Boolean(x => x.SurfaceWaterSupply, x => x.SurfaceWaterSupply);
            test.Boolean(x => x.Reservoir, x => x.Reservoir);
            test.Boolean(x => x.Dam, x => x.Dam);
            test.Boolean(x => x.Interconnection, x => x.Interconnection);
            test.Boolean(x => x.WaterTreatmentFacility, x => x.WaterTreatmentFacility);
            test.Boolean(x => x.DPCC, x => x.DPCC);
            //RMP/PointOfEntry are conditional fields, When tested with a bool value of true, another string field becomes required and causes an error.
            //but this file requires all props to tested. TestedElsewhere will let this pass - Greg - 1/31/2020, ARR 11/17/2020
            test.TestedElsewhere(x => x.RMP);
            test.TestedElsewhere(x => x.PointOfEntry);
            test.Boolean(x => x.Filtration, x => x.Filtration);
            test.Boolean(x => x.ResidualsGeneration, x => x.ResidualsGeneration);
            test.Boolean(x => x.TReport, x => x.TReport);
            test.Boolean(x => x.DistributivePumping, x => x.DistributivePumping);
            test.Boolean(x => x.BoosterPumping, x => x.BoosterStation);
            test.Boolean(x => x.PressureReducing, x => x.PressureReducing);
            test.Boolean(x => x.GroundStorage, x => x.GroundStorage);
            test.Boolean(x => x.ElevatedStorage, x => x.ElevatedStorage);
            test.Boolean(x => x.OnSiteAnalyticalInstruments, x => x.OnSiteAnalyticalInstruments);
            test.Boolean(x => x.SampleStation, x => x.SampleStation);
            test.Boolean(x => x.SewerLift, x => x.SewerLiftStation);
            test.Boolean(x => x.SewerTreatment, x => x.WasteWaterTreatmentFacility);
            test.Boolean(x => x.FieldOperations, x => x.FieldOperations);
            test.Boolean(x => x.SpoilsStaging, x => x.SpoilsStaging);
            test.Boolean(x => x.CellularAntenna, x => x.CellularAntenna);
            test.Boolean(x => x.SCADAIntrusionAlarm, x => x.SCADAIntrusionAlarm);
            test.Boolean(x => x.PSM, x => x.PSM);
            test.Boolean(x => x.UsedInProductionCapacityCalculation, x => x.UsedInProductionCapacityCalculation);
            test.Boolean(x => x.IgnitionEnterprisePortal, x => x.IgnitionEnterprisePortal);
            test.Boolean(x => x.ArcFlashLabelRequired, x => x.ArcFlashLabelRequired);

            test.TestedElsewhere(x => x.Id);
            test.TestedElsewhere(x => x.ChemicalFeed);
            test.TestedElsewhere(x => x.ChemicalStorageLocation);

            test.NotMapped(x => x.FacilityName);
        }

        [TestMethod]
        public void TestRecordIsLookedUpById()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(FACILITY_ID, _target.MapToEntity(uow, 2, MappingHelper).Id);
            });
        }

        #region Chemical Feed

        [TestMethod]
        public void TestChemicalFeedIsMappedFromChemicalFeed()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.ChemicalFeed, _target.MapToEntity(uow, 1, MappingHelper).ChemicalFeed);
            });
        }

        #endregion

        #region Chemical Storage Location

        [TestMethod]
        public void TestChemicalStorageLocationIsMappedFromChemicalStorageLocation()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(_target.ChemicalStorageLocation, _target.MapToEntity(uow, 1, MappingHelper).ChemicalStorageLocation);
            });
        }

        #endregion

        #region ChemicalFeed/ChemicalStorageLocation

        [TestMethod]
        public void TestThrowsWhenChemicalFeedIsTrueAndChemicalStorageLocationNotSpecified()
        {
            _target.ChemicalFeed = true;
            _target.ChemicalStorageLocation = null;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }

        #endregion
    }
}