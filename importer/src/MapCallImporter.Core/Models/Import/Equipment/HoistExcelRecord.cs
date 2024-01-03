using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HoistExcelRecord : EquipmentExcelRecordBase<HoistExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 166,
                         EQUIPMENT_PURPOSE = 363;

        public struct Characteristics
        {
            public const int CAPACITY_RATING = 1733,
                             CAPACITY_UOM = 1042,
                             HOIST_TYP = 1267,
                             NARUC_MAINTENANCE_ACCOUNT = 2098,
                             NARUC_OPERATIONS_ACCOUNT = 2099,
                             OWNED_BY = 1732,
                             RETEST_REQUIRED = 1064,
                             SPECIAL_MAINT_NOTES = 1734;
        }

        #endregion

        #region Properties

        public string CapacityRating { get; set; }
        public string CapacityUOM { get; set; }
        public string HoistType { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "HOIST";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2364;
        protected override int NARUCSpecialMtnNoteDetailsId => 2365;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(CapacityRating, nameof(CapacityRating), Characteristics.CAPACITY_RATING),
                mapper.DropDown(CapacityUOM, nameof(CapacityUOM), Characteristics.CAPACITY_UOM),
                mapper.DropDown(HoistType, nameof(HoistType), Characteristics.HOIST_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}