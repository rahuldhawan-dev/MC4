using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class CoolingTowerExcelRecordTest : EquipmentExcelRecordTestBase<CoolingTowerExcelRecord>
    {
        #region Private Methods

        protected override CoolingTowerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-CLTG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<CoolingTowerExcelRecord> test)
        {
            test.DropDown(x => x.HVACCoolingTower, "HVAC-TWR_TYP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.Application, "APPLICATION_HVAC-TWR");
            test.DropDown(x => x.EnergyType, "ENERGY_TP");
            test.String(x => x.OutputValue, "OUTPUT_VALUE");
            test.DropDown(x => x.OutputUOM, "OUTPUT_UOM");
            test.String(x => x.CFM, "CFM");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccounut, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.SpecialMtnNoteDetails);
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
