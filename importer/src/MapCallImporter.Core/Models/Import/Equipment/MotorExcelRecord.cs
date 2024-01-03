using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class MotorExcelRecord : EquipmentExcelRecordBase<MotorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 183;
        public const int EQUIPMENT_PURPOSE = 378;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_MOT = 1572,
                             DUTY_CYCLE = 1247,
                             FULL_LOAD_AMPS = 1605,
                             HP_RATING = 853,
                             INSULATION_CLASS = 1238,
                             MOT_ANTI_REVERSE = 1475,
                             MOT_BEARING_COUP_END = 1611,
                             MOT_BEARING_FREE_END = 1610,
                             MOT_BEARING_TP_COUP_END = 1400,
                             MOT_BEARING_TP_FREE_END = 965,
                             MOT_CATALOG_NUMBER = 1612,
                             MOT_CODE = 1466,
                             MOT_COUPLING_TP = 1145,
                             MOT_ENCLOSURE_TP = 1373,
                             MOT_EXCITATION_VOLTAGE = 1609,
                             MOT_FRAME_TP = 1607,
                             MOT_HOLLOW_SHAFT = 1249,
                             MOT_INVERTER_DUTY = 1510,
                             MOT_LUBE_TP_COUP_END = 1153,
                             MOT_LUBE_TP_FREE_END = 1210,
                             MOT_NAMEPLATE_DESIGN = 1275,
                             MOT_SERVICE_FACTOR = 953,
                             MOT_TYP = 942,
                             MOT_VOLTAGE_RUNNING = 885,
                             NARUC_MAINTENANCE_ACCOUNT = 2132,
                             NARUC_OPERATIONS_ACCOUNT = 2133,
                             NARUC_SPECIAL_MAINT_NOTE_DETAILS = 2399,
                             NARUC_SPECIAL_MAINT_NOTES = 2398,
                             ORIENTATION = 1567,
                             OWNED_BY = 1604,
                             ROTATION_DIRECTION = 1519,
                             RPM_OPERATING = 1606,
                             RPM_RATING = 1193,
                             SPECIAL_MAINT_NOTES_DIST = 1613,
                             TEMPERATURE_RISE = 1608,
                             VOLT_RATING = 1166;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string DutyCycle { get; set; }
        public string FullLoadAmps { get; set; }
        public string HPRating { get; set; }
        public string InsulationClass { get; set; }
        public string MotAntiReverse { get; set; }
        public string MotBearingcoup { get; set; }
        public string MotBearingfree { get; set; }
        public string MotBearingTPcoup { get; set; }
        public string MotBearingTPfree { get; set; }
        public string MotCatalogNumber { get; set; }
        public string MotCode { get; set; }
        public string MotCouplingType { get; set; }
        public string MotEnclosureType { get; set; }
        public string MotExcitationVolta { get; set; }
        public string MotFrameType { get; set; }
        public string MotHollowShaft { get; set; }
        public string MotInverterDuty { get; set; }
        public string MotLubeTypefree { get; set; }
        public string MotLubeTypecoupe { get; set; }
        public string MotNameplateDesign { get; set; }
        public string MotServiceFactor { get; set; }
        public string MotorType { get; set; }
        public string Orientation { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string RPMRating { get; set; }
        public string RotationDirection { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TemperatureRise { get; set; }
        public string VoltRating { get; set; }
        public string VoltageRunning { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2398;
        protected override int NARUCSpecialMtnNoteDetailsId => 2399;

        protected override string EquipmentType => "MOTOR";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_MOT),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.String(FullLoadAmps, nameof(FullLoadAmps), Characteristics.FULL_LOAD_AMPS),
                mapper.Numerical(HPRating, nameof(HPRating), Characteristics.HP_RATING),
                mapper.DropDown(InsulationClass, nameof(InsulationClass), Characteristics.INSULATION_CLASS),
                mapper.DropDown(MotAntiReverse, nameof(MotAntiReverse), Characteristics.MOT_ANTI_REVERSE),
                mapper.String(MotBearingcoup, nameof(MotBearingcoup), Characteristics.MOT_BEARING_COUP_END),
                mapper.String(MotBearingfree, nameof(MotBearingfree), Characteristics.MOT_BEARING_FREE_END),
                mapper.DropDown(MotBearingTPcoup, nameof(MotBearingTPcoup), Characteristics.MOT_BEARING_TP_COUP_END),
                mapper.DropDown(MotBearingTPfree, nameof(MotBearingTPfree), Characteristics.MOT_BEARING_TP_FREE_END),
                mapper.String(MotCatalogNumber, nameof(MotCatalogNumber), Characteristics.MOT_CATALOG_NUMBER),
                mapper.DropDown(MotCode, nameof(MotCode), Characteristics.MOT_CODE),
                mapper.DropDown(MotCouplingType, nameof(MotCouplingType), Characteristics.MOT_COUPLING_TP),
                mapper.DropDown(MotEnclosureType, nameof(MotEnclosureType), Characteristics.MOT_ENCLOSURE_TP),
                mapper.String(MotExcitationVolta, nameof(MotExcitationVolta), Characteristics.MOT_EXCITATION_VOLTAGE),
                mapper.String(MotFrameType, nameof(MotFrameType), Characteristics.MOT_FRAME_TP),
                mapper.DropDown(MotHollowShaft, nameof(MotHollowShaft), Characteristics.MOT_HOLLOW_SHAFT),
                mapper.DropDown(MotInverterDuty, nameof(MotInverterDuty), Characteristics.MOT_INVERTER_DUTY),
                mapper.DropDown(MotLubeTypefree, nameof(MotLubeTypefree), Characteristics.MOT_LUBE_TP_FREE_END),
                mapper.DropDown(MotLubeTypecoupe, nameof(MotLubeTypecoupe), Characteristics.MOT_LUBE_TP_COUP_END),
                mapper.DropDown(MotNameplateDesign, nameof(MotNameplateDesign), Characteristics.MOT_NAMEPLATE_DESIGN),
                mapper.DropDown(MotorType, nameof(MotorType), Characteristics.MOT_TYP),
                mapper.DropDown(MotServiceFactor, nameof(MotServiceFactor), Characteristics.MOT_SERVICE_FACTOR),
                mapper.DropDown(Orientation, nameof(Orientation), Characteristics.ORIENTATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RotationDirection, nameof(RotationDirection), Characteristics.ROTATION_DIRECTION),
                mapper.String(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.DropDown(RPMRating, nameof(RPMRating), Characteristics.RPM_RATING),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.Numerical(TemperatureRise, nameof(TemperatureRise), Characteristics.TEMPERATURE_RISE),
                mapper.DropDown(VoltageRunning, nameof(VoltageRunning), Characteristics.MOT_VOLTAGE_RUNNING),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}