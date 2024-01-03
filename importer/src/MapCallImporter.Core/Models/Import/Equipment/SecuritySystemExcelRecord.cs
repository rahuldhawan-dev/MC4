using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class SecuritySystemExcelRecord : EquipmentExcelRecordBase<SecuritySystemExcelRecord>
    {
        #region Constants

        public struct Characteristics
        {
            #region Constants

            public const int ACTION_TAKEN_UPON_ALARM = 940,
                             BACKUP_POWER = 1258,
                             OWNED_BY = 1978,
                             NARUC_MAINTENANCE_ACCOUNT = 2205,
                             NARUC_OPERATIONS_ACCOUNT = 2206,
                             RETEST_REQUIRED = 1217,
                             SECSYS_TYP = 1340;

            #endregion
        }

        public const int EQUIPMENT_TYPE = 216, EQUIPMENT_PURPOSE = 414;

        #endregion

        #region Properties

        public string ActionTakenUponAl { get; set; }
        public string BackupPower { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SecuritySystemType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2461;
        protected override int NARUCSpecialMtnNoteDetailsId => 2462;

        protected override string EquipmentType => "SECURITY SYSTEM";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ActionTakenUponAl, nameof(ActionTakenUponAl), Characteristics.ACTION_TAKEN_UPON_ALARM),
                mapper.DropDown(BackupPower, nameof(BackupPower), Characteristics.BACKUP_POWER),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.DropDown(SecuritySystemType, nameof(SecuritySystemType), Characteristics.SECSYS_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}