using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class VOCStripperExcelRecordTest : EquipmentExcelRecordTestBase<VOCStripperExcelRecord>
    {
        #region Private Methods

        protected override VOCStripperExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "WATER";
            ret.StripperType = "PACKED AIR TOWER";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-VOCG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<VOCStripperExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_TRT-STRP");
            test.String(x => x.BackwashRategpms, "BACKWASH_RATE");
            test.String(x => x.FlowNormalRange, "FLOW_NORMAL_RANGE");
            test.String(x => x.Media1Depth, "MEDIA_1_DEPTH");
            test.DropDown(x => x.IndoorOutdoor, "LOCATION");
            test.DropDown(x => x.MaterialofConstruc, "MATERIAL_OF_CONSTRUCTION");
            test.DropDown(x => x.Media1Type, "MEDIA_1_TP_TRT-STRP");
            test.DropDown(x => x.Media2Type, "MEDIA_2_TP_TRT-STRP");
            test.DropDown(x => x.MediaRegenerationR, "MEDIA_REGENERATION_REQD");
            test.DropDown(x => x.StripperType, "TRT-STRP_TYP");
            test.DropDown(x => x.WashType, "WASH_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.SurfaceAreasqft, "SURFACE_AREA");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.Media2Depth);
            test.NotMapped(x => x.SpecialMtnNoteDet);
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
