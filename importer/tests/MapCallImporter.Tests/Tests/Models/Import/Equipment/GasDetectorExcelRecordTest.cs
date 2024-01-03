using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class GasDetectorExcelRecordTest : EquipmentExcelRecordTestBase<GasDetectorExcelRecord>
    {
        #region Private Methods

        protected override GasDetectorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.ActionTakenUponAl = "SIREN";
            ret.GasDetectedMitiga = "CHLORINE";
            ret.GasDetectorType = "PERMANENT";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-GDTG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<GasDetectorExcelRecord> test)
        {
            test.DropDown(x => x.ActionTakenUponAl, "ACTION_TAKEN_UPON_ALARM");
            test.DropDown(x => x.BackupPower, "BACKUP_POWER");
            test.DropDown(x => x.GasDetectedMitiga, "GAS_MITIGATED");
            test.DropDown(x => x.GasDetectorType, "SAFGASDT_TYP");
            test.String(x => x.GasAlarmSetPoint, "GAS_ALARM_SETPOINT");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

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
