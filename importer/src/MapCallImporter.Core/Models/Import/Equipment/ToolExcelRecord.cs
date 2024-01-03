using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ToolExcelRecord : EquipmentExcelRecordBase<ToolExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 226,
                         EQUIPMENT_PURPOSE = 419;

        public struct Characteristics
        {
            public const int CAPACITY_RATING = 1790,
                             CAPACITY_UOM_E = 1005,
                             NARUC_MAINTENANCE_ACCOUNT = 2235,
                             NARUC_OPERATIONS_ACCOUNT = 2236,
                             OWNED_BY = 1789,
                             RETEST_REQUIRED = 1229,
                             SPECIAL_MAINT_NOTES = 1791,
                             TOOL_TYP = 999;
        }

        #endregion

        #region Properties

        public string CapacityRating { get; set; }
        public string CapacityUOM { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string ToolType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "TOOL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2481;
        protected override int NARUCSpecialMtnNoteDetailsId => 2482;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(CapacityRating, nameof(CapacityRating), Characteristics.CAPACITY_RATING),
                mapper.DropDown(CapacityUOM, nameof(CapacityUOM), Characteristics.CAPACITY_UOM_E),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.DropDown(ToolType, nameof(ToolType), Characteristics.TOOL_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}