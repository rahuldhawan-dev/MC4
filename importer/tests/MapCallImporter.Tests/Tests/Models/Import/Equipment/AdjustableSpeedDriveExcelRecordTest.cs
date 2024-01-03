using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class AdjustableSpeedDriveExcelRecordTest : EquipmentExcelRecordTestBase<AdjustableSpeedDriveExcelRecord>
    {
        #region Private Methods

        protected override AdjustableSpeedDriveExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.AmpRating = "100.00 A";
            ret.HPRating = "999.00 HP AW";
            ret.OwnedBy = "AW";
            ret.SpeedDriveType = "VFD";
            ret.VoltRating = "480";

            return ret;
        }

        #endregion

        protected override string ExpectedIdentifier => "NJ7-10-ASDG-1";

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<AdjustableSpeedDriveExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.Numerical(x => x.FullLoadAmps, "FULL_LOAD_AMPS");
            test.Numerical(x => x.HPRating, "HP_RATING");
            test.String(x => x.OwnedBy, "Owned By");
            test.DropDown(x => x.PulseType, "PULSE_TP");
            test.DropDown(x => x.SpeedDriveType, "ADJSPD_TYP");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_Operations_Account");

            test.NotMapped(e => e.SpecialMtnNoteDet);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        [TestMethod]
        public void TestLockoutRequiredPrerequisiteIsAdded()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                MyAssert.Contains(result.ProductionPrerequisites,
                    pp => pp.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
            });
        }

        #endregion
    }
}
