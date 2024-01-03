using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class FacilityAndGroundsExcelRecordTest : EquipmentExcelRecordTestBase<FacilityAndGroundsExcelRecord>
    {
        #region Private Methods

        protected override FacilityAndGroundsExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.FacilityType = "FACILITY*";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-FGDG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<FacilityAndGroundsExcelRecord> test)
        {
            test.DropDown(x => x.BackupPower, "BACKUP_POWER");
            test.Numerical(x => x.Block, "Block");
            test.DropDown(x => x.FacilityType, "FACILITY_TYP");
            test.DropDown(x => x.FireAlarm, "FACILITY_FIRE_ALARM");
            test.Numerical(x => x.Lot, "Lot");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.SecurityType, "FACILITY_SECURITY_TP");
            test.DropDown(x => x.Staffing, "FACILITY_STAFFING");
            test.String(x => x.TotalSqft, "TOTAL_SQ_FT");
            test.DropDown(x => x.VoltageEntering, "FACILITY_VOLTAGE");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.SpecialMtnNote);
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