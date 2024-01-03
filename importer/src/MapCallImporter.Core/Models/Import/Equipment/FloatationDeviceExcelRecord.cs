using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FloatationDeviceExcelRecord : EquipmentExcelRecordBase<FloatationDeviceExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 195,
                         EQUIPMENT_PURPOSE = 354;

        public struct Characteristics
        {
            public const int BACKUP_POWER = 1421,
                             NARUC_MAINTENANCE_ACCOUNT = 2157,
                             NARUC_OPERATIONS_ACCOUNT = 2158,
                             OWNED_BY = 2155,
                             PPE_RATING = 1488,
                             PPE_FLOT_TYP = 1334,
                             RETEST_REQUIRED = 1243,
                             SPECIAL_MAINT_NOTE = 2156;
        }

        #endregion

        #region Properties

        public string BackupPower { get; set; }
        public string FlotationType { get; set; }
        public string OwnedBy { get; set; }
        public string PPERating { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "FLOATATION DEVICE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2420;
        protected override int NARUCSpecialMtnNoteDetailsId => 2421;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(BackupPower, nameof(BackupPower), Characteristics.BACKUP_POWER),
                mapper.DropDown(FlotationType, nameof(FlotationType), Characteristics.PPE_FLOT_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PPERating, nameof(PPERating), Characteristics.PPE_RATING),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTE),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}