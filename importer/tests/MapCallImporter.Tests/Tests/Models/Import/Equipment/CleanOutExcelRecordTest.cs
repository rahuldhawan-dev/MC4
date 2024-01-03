using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class CleanOutExcelRecordTest : EquipmentExcelRecordTestBase<CleanOutExcelRecord>
    {
        #region Private Methods

        protected override CleanOutExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-CLOG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<CleanOutExcelRecord> test)
        {
            test.DropDown(x => x.CleanoutType, "CO_TYP");
            test.DropDown(x => x.WasteWaterType, "WASTE_WATER_TP");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_CO");
            test.String(x => x.DependencyDriver1, "DEPENDENCY_DRIVER_1");
            test.String(x => x.DependencyDriver2, "DEPENDENCY_DRIVER_2");
            test.DropDown(x => x.CleanoutSize, "CO_SIZE");
            test.DropDown(x => x.MaterialofConstruction, "MATERIAL_OF_CONSTRUCTION_CO");
            test.DropDown(x => x.SurfaceCover, "SURFACE_COVER");
            test.DropDown(x => x.SurfaceCoverLocType, "SURFACE_COVER_LOC_TP");
            test.DropDown(x => x.AssetLocation, "ASSET_LOCATION");
            test.DropDown(x => x.COFittingType, "CO_FITTING_TP");
            test.DropDown(x => x.COSweepType, "CO_SWEEP_TP");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.Basin);
            test.NotMapped(x => x.HistoricalID);
            test.NotMapped(x => x.InstallationWO);
            test.NotMapped(x => x.MapPage);
            test.NotMapped(x => x.OwnedBy);
            test.NotMapped(x => x.SpecialMtnNoteDetail);
            test.NotMapped(x => x.SubBasin);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        [TestMethod]
        public void TestLockoutRequiredPrerequisiteIsNotAdded()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                MyAssert.DoesNotContain(result.ProductionPrerequisites,
                    pp => pp.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
            });
        }

        #endregion
    }
}
