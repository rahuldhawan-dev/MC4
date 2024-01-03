using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class DefibrillatorExcelRecord : EquipmentExcelRecordBase<DefibrillatorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 122,
                         EQUIPMENT_PURPOSE = 339;

        public struct Characteristics
        {
            public const int AED_TYP = 1000,
                             BACKUP_POWER = 1159,
                             NARUC_Maintenance_Account = 2010,
                             NARUC_OPERATIONS_ACCOUNT = 2011,
                             OWNED_BY = 1693,
                             PPE_RATING = 1551,
                             RETEST_REQUIRED = 1568,
                             SPECIAL_MAINT_NOTES = 1694;
        }

        #endregion

        #region Properties

        public string AEDType { get; set; }
        public string BackupPower { get; set; }
        public string OwnedBy { get; set; }
        public string PPERating { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "DEFIBRILLATOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2276;
        protected override int NARUCSpecialMtnNoteDetailsId => 2277;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(AEDType, nameof(AEDType), Characteristics.AED_TYP),
                mapper.DropDown(BackupPower, nameof(BackupPower), Characteristics.BACKUP_POWER),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PPERating, nameof(PPERating), Characteristics.PPE_RATING),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_Maintenance_Account),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}