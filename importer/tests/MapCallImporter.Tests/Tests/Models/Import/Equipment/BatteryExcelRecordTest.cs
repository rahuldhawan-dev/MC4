using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class BatteryExcelRecordTest : EquipmentExcelRecordTestBase<BatteryExcelRecord>
    {
        #region Private Methods

        protected override BatteryExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.BatteryType = "MAINTAINED BATT";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-BATG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<BatteryExcelRecord> test)
        {
            test.Numerical(x => x.BatteriesinBank, "#_BATTERIES_BANK");
            test.String(x => x.AmpHoursea, "AMP_HOURS");
            test.DropDown(x => x.BatCellType, "BATT_CELL_TP");
            test.DropDown(x => x.BatteryType, "BATT_TYP");
            test.String(x => x.ColdCrankingAmpse, "COLD_CRANKING_AMPS");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.VoltRating, "BATT_VOLT_RATING");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_OPERATIONS_ACCOUNT");

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