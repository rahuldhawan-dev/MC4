using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class FlowMeterExcelRecordTest : EquipmentExcelRecordTestBase<FlowMeterExcelRecord>
    {
        #region Private Methods

        protected override FlowMeterExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.BypassValve = "Y";
            ret.FlowmeterType = "COMPOUND";
            ret.NominalSize = "10";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-FMNG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<FlowMeterExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_FLO-MET");
            test.String(x => x.BetaRatio, "BETA_RATIO");
            test.DropDown(x => x.BypassValve, "BYPASS_VALVE");
            test.DropDown(x => x.CalibrationUOM, "CALIBRATION_UOM");
            test.DropDown(x => x.CommProtocol, "COMM_PROTOCOL");
            test.String(x => x.DifferentialPSIRan, "DIFFERENTIAL_PSI_RANGE");
            test.String(x => x.FlowCalibratedRang, "FLOW_CALIBRATED_RANGE");
            test.String(x => x.FlowMaxIntermitten, "FLOW_MAX_INTERMITTENT");
            test.String(x => x.FlowNormalRange, "FLOW_NORMAL_RANGE");
            test.DropDown(x => x.FlowUOM, "FLOW_UOM");
            test.String(x => x.FlowVelocityRange, "FLOW_VELOCITY_RANGE");
            test.DropDown(x => x.FlowmeterType, "FLO-MET_TYP");
            test.DropDown(x => x.InstrumentPower, "INSTRUMENT_POWER");
            test.String(x => x.MaxPressure, "MAX_PRESSURE");
            test.DropDown(x => x.MetMaterial, "MET_MATERIAL");
            test.DropDown(x => x.NominalSize, "NOMINAL_SIZE");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.DropDown(x => x.OutputType, "OUTPUT_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.PressureDropMax, "PRESSURE_DROP_MAX");
            test.String(x => x.SpecificGravity, "SPECIFIC_GRAVITY");
            test.DropDown(x => x.SquareRoot, "SQUARE_ROOT");
            test.DropDown(x => x.TransmitterManufact, "TRANSMITTER_MANUFACTURER");
            test.DropDown(x => x.Transmitter, "TRANSMITTER");
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
