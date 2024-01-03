using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class InstrumentSwitchExcelRecordTest : EquipmentExcelRecordTestBase<InstrumentSwitchExcelRecord>
    {
        #region Private Methods

        protected override InstrumentSwitchExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-INSG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<InstrumentSwitchExcelRecord> test)
        {
            test.String(x => x.InstrumentRange, "INSTRUMENT_RANGE");
            test.DropDown(x => x.InstrumentUOM, "INSTRUMENT_UOM");
            test.DropDown(x => x.LoopPower, "LOOP_POWER");
            test.DropDown(x => x.NEMAEnclosure, "NEMA_ENCLOSURE");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.DropDown(x => x.OutputType, "OUTPUT_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.SwitchType, "INST-SW_TYP");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

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