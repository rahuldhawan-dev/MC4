using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class MotorExcelRecordTest : EquipmentExcelRecordTestBase<MotorExcelRecord>
    {
        #region Private Methods

        protected override MotorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.HPRating = "100.00 meh";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-MTRG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(EquipmentCharacteristicMappingTester<MotorExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_MOT");
            test.DropDown(x => x.DutyCycle, "DUTY_CYCLE");
            test.String(x => x.FullLoadAmps, "FULL_LOAD_AMPS");
            test.Numerical(x => x.HPRating, "HP_RATING");
            test.DropDown(x => x.InsulationClass, "INSULATION_CLASS");
            test.DropDown(x => x.MotAntiReverse, "MOT_ANTI_REVERSE");
            test.String(x => x.MotBearingcoup, "MOT_BEARING_COUP_END");
            test.String(x => x.MotBearingfree, "MOT_BEARING_FREE_END");
            test.DropDown(x => x.MotBearingTPcoup, "MOT_BEARING_TP_COUP_END");
            test.DropDown(x => x.MotBearingTPfree, "MOT_BEARING_TP_FREE_END");
            test.String(x => x.MotCatalogNumber, "MOT_CATALOG_NUMBER");
            test.DropDown(x => x.MotCode, "MOT_CODE");
            test.DropDown(x => x.MotCouplingType, "MOT_COUPLING_TP");
            test.DropDown(x => x.MotEnclosureType, "MOT_ENCLOSURE_TP");
            test.String(x => x.MotExcitationVolta, "MOT_EXCITATION_VOLTAGE");
            test.String(x => x.MotFrameType, "MOT_FRAME_TP");
            test.DropDown(x => x.MotHollowShaft, "MOT_HOLLOW_SHAFT");
            test.DropDown(x => x.MotInverterDuty, "MOT_INVERTER_DUTY");
            test.DropDown(x => x.MotLubeTypefree, "MOT_LUBE_TP_FREE_END");
            test.DropDown(x => x.MotLubeTypecoupe, "MOT_LUBE_TP_COUP_END");
            test.DropDown(x => x.MotNameplateDesign, "MOT_NAMEPLATE_DESIGN");
            test.DropDown(x => x.MotorType, "MOT_TYP");
            test.DropDown(x => x.MotServiceFactor, "MOT_SERVICE_FACTOR");
            test.DropDown(x => x.Orientation, "ORIENTATION");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.RotationDirection, "ROTATION_DIRECTION");
            test.String(x => x.RPMOperating, "RPM_OPERATING");
            test.DropDown(x => x.RPMRating, "RPM_RATING");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_DIST");
            test.Numerical(x => x.TemperatureRise, "TEMPERATURE_RISE");
            test.DropDown(x => x.VoltageRunning, "MOT_VOLTAGE_RUNNING");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
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
