using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class HVACVentilatorExcelRecordTest : EquipmentExcelRecordTestBase<HVACVentilatorExcelRecord>
    {
        #region Private Methods

        protected override HVACVentilatorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "COMFORT";
            ret.EnergyType = "ELECTRIC";
            ret.HVACVentilation = "HVAC-VNT*";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-HVVG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<HVACVentilatorExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.DropDown(x => x.Application, "APPLICATION_HVAC-VNT");
            test.DropDown(x => x.DutyCycle, "DUTY_CYCLE");
            test.DropDown(x => x.EnergyType, "ENERGY_TP");
            test.DropDown(x => x.HVACVentilation, "HVAC-VNT_TYP");
            test.DropDown(x => x.OutputUOM, "OUTPUT_UOM");
            test.String(x => x.OutputValue, "OUTPUT_VALUE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
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