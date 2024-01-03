using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class GasDetectorExcelRecord : EquipmentExcelRecordBase<GasDetectorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 210;
        public const int EQUIPMENT_PURPOSE = 358;

        public struct Characteristics
        {
            #region Constants

            public const int ACTION_TAKEN_UPON_ALARM = 1374,
                             BACKUP_POWER = 1504,
                             GAS_ALARM_SETPOINT = 2190,
                             GAS_MITIGATED = 1300,
                             NARUC_MAINTENANCE_ACCOUNT = 2192,
                             NARUC_OPERATIONS_ACCOUNT = 2193,
                             NARUC_SPECIAL_MAINT_NOTE_DETAILS = 2450,
                             NARUC_SPECIAL_MAINT_NOTES = 2449,
                             OWNED_BY = 2189,
                             SAFGASDT_TYP = 1223,
                             SPECIAL_MAINT_NOTES = 2191;

            #endregion
        }

        #endregion

        #region Properties

        public string ActionTakenUponAl { get; set; }
        public string BackupPower { get; set; }
        public string GasDetectedMitiga { get; set; }
        public string GasAlarmSetPoint { get; set; }
        public string GasDetectorType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2449;
        protected override int NARUCSpecialMtnNoteDetailsId => 2450;

        protected override string EquipmentType => "GAS DETECTOR";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(ActionTakenUponAl, nameof(ActionTakenUponAl), Characteristics.ACTION_TAKEN_UPON_ALARM),
                mapper.DropDown(BackupPower, nameof(BackupPower), Characteristics.BACKUP_POWER),
                mapper.DropDown(GasDetectedMitiga, nameof(GasDetectedMitiga), Characteristics.GAS_MITIGATED),
                mapper.DropDown(GasDetectorType, nameof(GasDetectorType), Characteristics.SAFGASDT_TYP),
                mapper.String(GasAlarmSetPoint, nameof(GasAlarmSetPoint), Characteristics.GAS_ALARM_SETPOINT),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}