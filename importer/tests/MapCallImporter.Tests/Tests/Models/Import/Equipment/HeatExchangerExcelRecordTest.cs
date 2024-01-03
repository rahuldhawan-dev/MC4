using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class HeatExchangerExcelRecordTest : EquipmentExcelRecordTestBase<HeatExchangerExcelRecord>
    {
        #region Private Methods

        protected override HeatExchangerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-HXCG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<HeatExchangerExcelRecord> test)
        {
            test.DropDown(x => x.HVACExchanger, "HVAC-EXC_TYP");
            test.DropDown(x => x.Material2, "MATERIAL_2");
            test.DropDown(x => x.MaterialofConstruc, "MATERIAL_OF_CONSTRUCTION_HVAC");
            test.String(x => x.MaxPressure, "MAX_PRESSURE");
            test.DropDown(x => x.Media1Type, "MEDIA_1_TP_HVAC-EXC");
            test.DropDown(x => x.Media2Type, "MEDIA_2_TP_HVAC-EXC");
            test.DropDown(x => x.OutputUOM, "OUTPUT_UOM");
            test.String(x => x.OutputValue, "OUTPUT_VALUE");
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
