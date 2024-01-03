using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ManholeExcelRecordTest : EquipmentExcelRecordTestBase<ManholeExcelRecord>
    {
        #region Private Methods

        protected override ManholeExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-MNHG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ManholeExcelRecord> test)
        {
            test.DropDown(x => x.AssetLocation, "ASSET_LOCATION");
            test.DropDown(x => x.BenchTrough, "MH_TROUGH");
            test.DropDown(x => x.ConeInsert, "MH_CONE_INSERT");
            test.DropDown(x => x.ConeMaterial, "MH_CONE_MATERIAL");
            test.DropDown(x => x.CoverMaterial, "MH_COVER_MATERIAL");
            test.DropDown(x => x.CoverSize, "MH_COVER_SIZE");
            test.String(x => x.DependencyDriver1, "DEPENDENCY_DRIVER_1");
            test.String(x => x.DependencyDriver2, "DEPENDENCY_DRIVER_2");
            test.DropDown(x => x.HasSteps, "MH_HAS_STEPS");
            test.DropDown(x => x.LidCoverType, "MH_LID_TP");
            test.DropDown(x => x.Lined, "LINED");
            test.Date(x => x.LinedDate, "LINED_DATE");
            test.DropDown(x => x.LocnSubjecttoPonding, "MH_PONDING");
            test.DropDown(x => x.MHAdjustingRingMatl, "MH_ADJUSTING_RING_MATL");
            test.DropDown(x => x.MHCastingMaterial, "MH_CASTING_MATERIAL");
            test.DropDown(x => x.MHDropType, "MH_DROP_TP");
            test.DropDown(x => x.MHPipeSealType, "MH_PIPE_SEAL_TP");
            test.DropDown(x => x.ManholeSize, "MH_SIZE");
            test.DropDown(x => x.ManholeType, "MH_TYP");
            test.DropDown(x => x.MaterialofConstruction, "MATERIAL_OF_CONSTRUCTION_MH");
            test.DropDown(x => x.StepMaterial, "MH_STEP_MATERIAL");
            test.DropDown(x => x.SurfaceCover, "SURFACE_COVER");
            test.DropDown(x => x.SurfaceCoverLocType, "SURFACE_COVER_LOC_TP");
            test.DropDown(x => x.WasteWaterType, "WASTE_WATER_TP");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.Basin);
            test.NotMapped(x => x.ConeConfiguration);
            test.NotMapped(x => x.CoverLabel);
            test.NotMapped(x => x.GEOACCURACYGISDATA);
            test.NotMapped(x => x.HistoricalID);
            test.NotMapped(x => x.InstallationWO);
            test.NotMapped(x => x.MapPage);
            test.NotMapped(x => x.MHDepth);
            test.NotMapped(x => x.MHofSteps);
            test.NotMapped(x => x.OwnedBy);
            test.NotMapped(x => x.SpecialMtnNote);
            test.NotMapped(x => x.SpecialMtnNoteDet);
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
