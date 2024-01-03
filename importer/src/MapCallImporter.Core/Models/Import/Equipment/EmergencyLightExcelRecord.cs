using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class EmergencyLightExcelRecord : EquipmentExcelRecordBase<EmergencyLightExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 153,
                         EQUIPMENT_PURPOSE = 344;

        public struct Characteristics
        {
            public const int BACKUP_POWER = 1341,
                             ELIGHT_TYP = 1481,
                             NARUC_MAINTENANCE_ACCOUNT = 2072,
                             NARUC_OPERATIONS_ACCOUNT = 2073,
                             OWNED_BY = 1737,
                             RETEST_REQUIRED = 893,
                             SPECIAL_MAINT_NOTES = 1738;
        }

        #endregion

        #region Properties

        public string BackupPower { get; set; }
        public string EmergencyLightType { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "EMERGENCY LIGHT";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2338;
        protected override int NARUCSpecialMtnNoteDetailsId => 2339;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(BackupPower, nameof(BackupPower), Characteristics.BACKUP_POWER),
                mapper.DropDown(EmergencyLightType, nameof(EmergencyLightType), Characteristics.ELIGHT_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}