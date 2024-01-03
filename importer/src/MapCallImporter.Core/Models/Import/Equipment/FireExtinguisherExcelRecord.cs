using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FireExtinguisherExcelRecord : EquipmentExcelRecordBase<FireExtinguisherExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 158,
                         EQUIPMENT_PURPOSE = 351;

        public struct Characteristics
        {
            public const int FIRE_CLASS_RATING = 1422,
                             FIRE_EX_TYP = 1085,
                             NARUC_MAINTENANCE_ACCOUNT = 2082,
                             NARUC_OPERATIONS_ACCOUNT = 2083,
                             OWNED_BY = 1905,
                             RETEST_REQUIRED = 954;
        }

        #endregion

        #region Properties

        public string FireClassRating { get; set; }
        public string FireExtinguisherTy { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "FIRE EXTINGUISHER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2348;
        protected override int NARUCSpecialMtnNoteDetailsId => 2349;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(FireClassRating, nameof(FireClassRating), Characteristics.FIRE_CLASS_RATING),
                mapper.DropDown(FireExtinguisherTy, nameof(FireExtinguisherTy), Characteristics.FIRE_EX_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}