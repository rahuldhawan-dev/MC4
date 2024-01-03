using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class GravitySewerMainExcelRecordTest : EquipmentExcelRecordTestBase<GravitySewerMainExcelRecord>
    {
        #region Private Methods

        protected override GravitySewerMainExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-GSMG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<GravitySewerMainExcelRecord> test)
        {
            test.DropDown(x => x.GravityMainType, "GMAIN_TYP");
            test.DropDown(x => x.WasteWaterType, "WASTE_WATER_TP");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_GM");
            test.String(x => x.DependencyDriver1, "DEPENDENCY_DRIVER_1");
            test.DropDown(x => x.Lined, "LINED");
            test.Date(x => x.LinedDate, "LINED_DATE");
            test.DropDown(x => x.LinedMaterial, "LINED_MATERIAL");
            test.DropDown(x => x.AssetLocation, "ASSET_LOCATION");
            test.DropDown(x => x.SurfaceCoverLocType, "SURFACE_COVER_LOC_TP");
            test.DropDown(x => x.FlowDirection, "FLOW_DIRECTION");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.Basin);
            test.NotMapped(x => x.BookPage);
            test.NotMapped(x => x.DownstreamID);
            test.NotMapped(x => x.InspectionFrequency);
            test.NotMapped(x => x.InstallationWO);
            test.NotMapped(x => x.MapPage);
            test.NotMapped(x => x.OwnedBy);
            test.NotMapped(x => x.PipeChannelSize);
            test.NotMapped(x => x.PipeMaterial);
            test.NotMapped(x => x.SpecialMtnNoteDetails);
            test.NotMapped(x => x.SubBasin);
            test.NotMapped(x => x.UpstreamID);
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
