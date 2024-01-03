using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class FlowWeirExcelRecordTest : EquipmentExcelRecordTestBase<FlowWeirExcelRecord>
    {
        #region Private Methods

        protected override FlowWeirExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-FLWG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<FlowWeirExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_FLO-WEIR");
            test.DropDown(x => x.BypassValve, "BYPASS_VALVE");
            test.DropDown(x => x.CalibrationUOM, "CALIBRATION_UOM");
            test.DropDown(x => x.CommProtocol, "COMM_PROTOCOL");
            test.DropDown(x => x.FlowUOM, "FLOW_UOM");
            test.DropDown(x => x.FlowWeirType, "FLO-WEIR_TYP");
            test.DropDown(x => x.InstrumentPower, "INSTRUMENT_POWER");
            test.DropDown(x => x.MetMaterial, "MET_MATERIAL");
            test.DropDown(x => x.NominalSize, "NOMINAL_SIZE");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.DropDown(x => x.OutputType, "OUTPUT_TP");
            test.DropDown(x => x.SquareRoot, "SQUARE_ROOT");
            test.DropDown(x => x.TransmitterManufact, "TRANSMITTER_MANUFACTURER");
            test.DropDown(x => x.Transmitter, "TRANSMITTER");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.BetaRatio);
            test.NotMapped(x => x.DifferentialPSIRan);
            test.NotMapped(x => x.FlowCalibratedRang);
            test.NotMapped(x => x.FlowMaxIntermitten);
            test.NotMapped(x => x.FlowNormalRange);
            test.NotMapped(x => x.FlowVelocityRange);
            test.NotMapped(x => x.MaxPressure);
            test.NotMapped(x => x.OwnedBy);
            test.NotMapped(x => x.PressureDropMax);
            test.NotMapped(x => x.SpecialMtnNote);
            test.NotMapped(x => x.SpecialMtnNoteDet);
            test.NotMapped(x => x.SpecificGravity);
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
