using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PlantValveExcelRecord : EquipmentExcelRecordBase<PlantValveExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 199, EQUIPMENT_PURPOSE = 388;

        public struct Characteristics
        {
            #region Constants

            public const int ACTUATOR_TP = 1017,
                             APPLICATION_PVLV = 1447,
                             AUTOMATED_ACTUATED = 1225,
                             BYPASS_VALVE = 1059,
                             GEAR_TP = 1396,
                             JOINT_TP = 1148,
                             MATERIAL_OF_CONSTRUCTION_PVLV = 927,
                             NORMAL_POSITION = 1544,
                             NORMAL_SYS_PRESSURE = 859,
                             NUMBER_OF_TURNS = 863,
                             OPEN_CLOSE_SWITCHES = 1955,
                             OPEN_DIRECTION = 1338,
                             OWNED_BY = 1954,
                             PRESSURE_CLASS = 1427,
                             NARUC_MAINTENANCE_ACCOUNT = 2167,
                             NARUC_OPERATIONS_ACCOUNT = 2168,
                             PVLV_TYP = 1563,
                             VLV_ACTUATOR_MANUF = 1358,
                             VLV_FAIL_POSITION = 1590,
                             VLV_SWITCH = 1263,
                             VLV_VALVE_SIZE = 939,
                             VLV_VALVE_TP = 1200;

            #endregion
        }

        #endregion

        #region Properties

        public string ActuatorManufacture { get; set; }
        public string ActuatorType { get; set; }
        public string Application { get; set; }
        public string AutomatedActuated { get; set; }
        public string BypassValve { get; set; }
        public string FailPosition { get; set; }
        public string GearType { get; set; }
        public string JointType { get; set; }
        public string MaterialofConstruc { get; set; }
        public string NormalPosition { get; set; }
        public string NormalSysPressure { get; set; }
        public string NumberofTurns { get; set; }
        public string OpenDirection { get; set; }
        public string OpenCloseSwitches { get; set; }
        public string OwnedBy { get; set; }
        public string PlantValveType { get; set; }
        public string PressureClass { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VlvValveSize { get; set; }
        public string VlvValveType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }
        public string NARUCSpecialMtnNote { get; set; }
        public string NARUCSpecialMtnNoteDet { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2428;
        protected override int NARUCSpecialMtnNoteDetailsId => 2429;

        protected override string EquipmentType => "PLANT VALVE";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ActuatorManufacture, nameof(ActuatorManufacture), Characteristics.VLV_ACTUATOR_MANUF),
                mapper.DropDown(ActuatorType, nameof(ActuatorType), Characteristics.ACTUATOR_TP),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_PVLV),
                mapper.DropDown(AutomatedActuated, nameof(AutomatedActuated), Characteristics.AUTOMATED_ACTUATED),
                mapper.DropDown(BypassValve, nameof(BypassValve), Characteristics.BYPASS_VALVE),
                mapper.DropDown(FailPosition, nameof(FailPosition), Characteristics.VLV_FAIL_POSITION),
                mapper.DropDown(GearType, nameof(GearType), Characteristics.GEAR_TP),
                mapper.DropDown(JointType, nameof(JointType), Characteristics.JOINT_TP),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION_PVLV),
                mapper.DropDown(NormalPosition, nameof(NormalPosition), Characteristics.NORMAL_POSITION),
                mapper.Numerical(NormalSysPressure, nameof(NormalSysPressure), Characteristics.NORMAL_SYS_PRESSURE),
                mapper.Numerical(NumberofTurns, nameof(NumberofTurns), Characteristics.NUMBER_OF_TURNS),
                mapper.DropDown(OpenDirection, nameof(OpenDirection), Characteristics.OPEN_DIRECTION),
                mapper.String(OpenCloseSwitches, nameof(OpenCloseSwitches), Characteristics.OPEN_CLOSE_SWITCHES),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PlantValveType, nameof(PlantValveType), Characteristics.PVLV_TYP),
                mapper.DropDown(PressureClass, nameof(PressureClass), Characteristics.PRESSURE_CLASS),
                mapper.DropDown(VlvValveSize, nameof(VlvValveSize), Characteristics.VLV_VALVE_SIZE),
                mapper.DropDown(VlvValveType, nameof(VlvValveType), Characteristics.VLV_VALVE_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}