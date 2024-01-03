using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PressureDamperExcelRecord : EquipmentExcelRecordBase<PressureDamperExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 197,
                         EQUIPMENT_PURPOSE = 399;

        public struct Characteristics
        {
            public const int NARUC_MAINTENANCE_ACCOUNT = 2163,
                             NARUC_OPERATIONS_ACCOUNT = 2164,
                             OWNED_BY = 2161,
                             PRESDMP_TYP = 1287,
                             SPECIAL_MAINT_NOTES = 2162;
        }

        #endregion

        #region Properties

        public string OwnedBy { get; set; }
        public string PressureDamperType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "PRESSURE DAMPER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2424;
        protected override int NARUCSpecialMtnNoteDetailsId => 2425;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PressureDamperType, nameof(PressureDamperType), Characteristics.PRESDMP_TYP),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}