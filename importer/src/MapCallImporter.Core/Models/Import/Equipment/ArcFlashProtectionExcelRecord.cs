using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ArcFlashProtectionExcelRecord : EquipmentExcelRecordBase<ArcFlashProtectionExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 193,
                         EQUIPMENT_PURPOSE = 316;

        public struct Characteristics
        {
            public const int NARUC_MAINTENANCE_ACCOUNT = 2151,
                             NARUC_OPERATIONS_ACCOUNT = 2152,
                             OWNED_BY = 1742,
                             PPE_RATING = 1562,
                             PPE_ARC_TYP = 1541,
                             RETEST_REQUIRED = 1227,
                             SPECIAL_MAINT_NOTES = 1743;
        }

        #endregion

        #region Properties

        public string ArcFlashGearType { get; set; }
        public string OwnedBy { get; set; }
        public string PPERating { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "ARC FLASH PROTECTION";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2416;
        protected override int NARUCSpecialMtnNoteDetailsId => 2417;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ArcFlashGearType, nameof(ArcFlashGearType), Characteristics.PPE_ARC_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PPERating, nameof(PPERating), Characteristics.PPE_RATING),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT)
            };
        }

        #endregion
    }
}