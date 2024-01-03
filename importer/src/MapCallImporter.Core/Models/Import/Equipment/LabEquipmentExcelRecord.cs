using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class LabEquipmentExcelRecord : EquipmentExcelRecordBase<LabEquipmentExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 179,
                         EQUIPMENT_PURPOSE = 373;

        public struct Characteristics
        {
            public const int LABEQ_TYP = 1208,
                             NARUC_MAINTENANCE_ACCOUNT = 2124,
                             NARUC_OPERATIONS_ACCOUNT = 2125,
                             OWNED_BY = 1774,
                             RETEST_REQUIRED = 1511,
                             SPECIAL_MAINT_NOTES = 1775;
        }

        #endregion

        #region Properties

        public string LabEquipType { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "LAB EQUIPMENT";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2390;
        protected override int NARUCSpecialMtnNoteDetailsId => 2391;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(LabEquipType, nameof(LabEquipType), Characteristics.LABEQ_TYP),
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