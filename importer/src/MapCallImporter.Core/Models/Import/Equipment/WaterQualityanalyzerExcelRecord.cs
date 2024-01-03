using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class WaterQualityAnalyzerExcelRecord : EquipmentExcelRecordBase<WaterQualityAnalyzerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 238, EQUIPMENT_PURPOSE = 429;

        public struct Characteristics
        {
            #region Constants

            public const int COMM_PROTOCOL = 1274,
                             HIGH_ALARM_SETPOINT = 1831,
                             LOOP_POWER = 1470,
                             LOW_ALARM_SETPOINT = 1832,
                             MAX_PRESSURE = 1833,
                             NEMA_ENCLOSURE = 938,
                             ON_SCADA = 1382,
                             OUTPUT_TP = 979,
                             NARUC_MAINTENANCE_ACCOUNT = 2259,
                             NARUC_OPERATIONS_ACCOUNT = 2260,
                             OWNED_BY = 1830,
                             SPECIAL_MAINT_NOTES = 1834,
                             STANDBY_POWER_TP = 1120,
                             TRANSMITTER = 1155,
                             VOLT_RATING = 1023,
                             WQ_TEMPARATURE = 1020,
                             WQANLZR_TYP = 1125;

            #endregion
        }

        #endregion

        #region Properties

        public string AnalyzerType { get; set; }
        public string CommProtocol { get; set; }
        public string HighAlarmSetPoint { get; set; }
        public string LoopPower { get; set; }
        public string LowAlarmSetPoint { get; set; }
        public string MaxPressure { get; set; }
        public string NEMAEnclosure { get; set; }
        public string OnSCADA { get; set; }
        public string OutputType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string Transmitter { get; set; }
        public string VoltRating { get; set; }
        public string WQTemperature { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2505;
        protected override int NARUCSpecialMtnNoteDetailsId => 2506;

        protected override string EquipmentType => "WATER QUALITY ANALYZER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(AnalyzerType, nameof(AnalyzerType), Characteristics.WQANLZR_TYP),
                mapper.DropDown(CommProtocol, nameof(CommProtocol), Characteristics.COMM_PROTOCOL),
                mapper.String(HighAlarmSetPoint, nameof(HighAlarmSetPoint), Characteristics.HIGH_ALARM_SETPOINT),
                mapper.DropDown(LoopPower, nameof(LoopPower), Characteristics.LOOP_POWER),
                mapper.String(LowAlarmSetPoint, nameof(LowAlarmSetPoint), Characteristics.LOW_ALARM_SETPOINT),
                mapper.String(MaxPressure, nameof(MaxPressure), Characteristics.MAX_PRESSURE),
                mapper.DropDown(NEMAEnclosure, nameof(NEMAEnclosure), Characteristics.NEMA_ENCLOSURE),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OutputType, nameof(OutputType), Characteristics.OUTPUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.DropDown(Transmitter, nameof(Transmitter), Characteristics.TRANSMITTER),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.DropDown(WQTemperature, nameof(WQTemperature), Characteristics.WQ_TEMPARATURE),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}