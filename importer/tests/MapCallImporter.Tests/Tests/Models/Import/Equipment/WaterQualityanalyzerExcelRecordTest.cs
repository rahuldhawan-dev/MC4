using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class WaterQualityAnalyzerExcelRecordTest : EquipmentExcelRecordTestBase<WaterQualityAnalyzerExcelRecord>
    {
        #region Private Methods

        protected override WaterQualityAnalyzerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.AnalyzerType = "CHLORAMINE ANLYZR(C)";
            ret.OnSCADA = "Y";
            ret.OwnedBy = "AW";
            ret.Transmitter = "Y";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-WQAG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<WaterQualityAnalyzerExcelRecord> test)
        {
            test.DropDown(x => x.AnalyzerType, "WQANLZR_TYP");
            test.DropDown(x => x.CommProtocol, "COMM_PROTOCOL");
            test.String(x => x.HighAlarmSetPoint, "HIGH_ALARM_SETPOINT");
            test.DropDown(x => x.LoopPower, "LOOP_POWER");
            test.String(x => x.LowAlarmSetPoint, "LOW_ALARM_SETPOINT");
            test.String(x => x.MaxPressure, "MAX_PRESSURE");
            test.DropDown(x => x.NEMAEnclosure, "NEMA_ENCLOSURE");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.DropDown(x => x.OutputType, "OUTPUT_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.StandbyPowerType, "STANDBY_POWER_TP");
            test.DropDown(x => x.Transmitter, "TRANSMITTER");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.DropDown(x => x.WQTemperature, "WQ_TEMPARATURE");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");

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