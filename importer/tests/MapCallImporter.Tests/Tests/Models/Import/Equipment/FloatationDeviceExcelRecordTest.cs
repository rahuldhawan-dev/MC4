using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class FloatationDeviceExcelRecordTest : EquipmentExcelRecordTestBase<FloatationDeviceExcelRecord>
    {
        #region Private Methods

        protected override FloatationDeviceExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-FLDG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<FloatationDeviceExcelRecord> test)
        {
            test.DropDown(x => x.BackupPower, "BACKUP_POWER");
            test.DropDown(x => x.FlotationType, "PPE-FLOT_TYP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.PPERating, "PPE_RATING");
            test.DropDown(x => x.RetestRequired, "RETEST_REQUIRED");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTE");
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
