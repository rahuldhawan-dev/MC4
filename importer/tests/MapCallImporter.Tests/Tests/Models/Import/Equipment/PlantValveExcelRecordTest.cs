using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class PlantValveExcelRecordTest : EquipmentExcelRecordTestBase<PlantValveExcelRecord>
    {
        #region Private Methods

        protected override PlantValveExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.NormalPosition = "CLOSED";
            ret.NumberofTurns = "100.00 meh";
            ret.OpenDirection = "LEFT";
            ret.OwnedBy = "AW";
            ret.PlantValveType = "BACKFLOW W-TEST PORTS PVLV";
            ret.PressureClass = "150";
            ret.VlvValveSize = "0.25";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-PLVG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<PlantValveExcelRecord> test)
        {
            test.DropDown(x => x.ActuatorManufacture, "VLV_ACTUATOR_MANUF");
            test.DropDown(x => x.ActuatorType, "ACTUATOR_TP");
            test.DropDown(x => x.Application, "APPLICATION_PVLV");
            test.DropDown(x => x.AutomatedActuated, "AUTOMATED_ACTUATED");
            test.DropDown(x => x.BypassValve, "BYPASS_VALVE");
            test.DropDown(x => x.FailPosition, "VLV_FAIL_POSITION");
            test.DropDown(x => x.GearType, "GEAR_TP");
            test.DropDown(x => x.JointType, "JOINT_TP");
            test.DropDown(x => x.MaterialofConstruc, "MATERIAL_OF_CONSTRUCTION_PVLV");
            test.DropDown(x => x.NormalPosition, "NORMAL_POSITION");
            test.Numerical(x => x.NormalSysPressure, "NORMAL_SYS_PRESSURE");
            test.Numerical(x => x.NumberofTurns, "NUMBER_OF_TURNS");
            test.DropDown(x => x.OpenDirection, "OPEN_DIRECTION");
            test.String(x => x.OpenCloseSwitches, "OPEN/CLOSE SWITCHES");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.PlantValveType, "PVLV_TYP");
            test.DropDown(x => x.PressureClass, "PRESSURE_CLASS");
            test.DropDown(x => x.VlvValveSize, "VLV_VALVE_SIZE");
            test.DropDown(x => x.VlvValveType, "VLV_VALVE_TP");
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