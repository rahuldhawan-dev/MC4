using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FallProtectionExcelRecord : EquipmentExcelRecordBase<FallProtectionExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 194, EQUIPMENT_PURPOSE = 348;

        public struct Characteristics
        {
            #region Constants

            public const int NARUC_MAINTENANCE_ACCOUNT = 2153,
                             NARUC_OPERATIONS_ACCOUNT = 2154,
                             OWNED_BY = 1744,
                             PPE_RATING = 1365,
                             PPE_FALL_TYP = 1333,
                             RETEST_REQUIRED = 1401,
                             SPECIAL_MAINT_NOTES = 1745;

            #endregion
        }

        #endregion

        #region Properties

        public string FallProtectionType { get; set; }
        public string OwnedBy { get; set; }
        public string PPERating { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2418;
        protected override int NARUCSpecialMtnNoteDetailsId => 2419;

        protected override string EquipmentType => "FALL PROTECTION";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(FallProtectionType, nameof(FallProtectionType), Characteristics.PPE_FALL_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PPERating, nameof(PPERating), Characteristics.PPE_RATING),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}