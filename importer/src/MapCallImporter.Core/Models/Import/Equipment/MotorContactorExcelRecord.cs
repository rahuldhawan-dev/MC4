using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class MotorContactorExcelRecord : EquipmentExcelRecordBase<MotorContactorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 146,
                         EQUIPMENT_PURPOSE = 379;

        public struct Characteristics
        {
            public const int AMP_RATING = 815,
                             CONTACTOR_TP = 1367,
                             CONTACTR_TYP = 1343,
                             INTERRUPTING_CAPACITY = 1747,
                             NARUC_MAINTENANCE_ACCOUNT = 2058,
                             NARUC_OPERATIONS_ACCOUNT = 2059,
                             OWNED_BY = 1746,
                             SPECIAL_MAINT_NOTES = 1748,
                             VOLT_RATING = 1099;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string ContactorType { get; set; }
        public string InterruptingCapacit { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "MOTOR CONTACTOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2324;
        protected override int NARUCSpecialMtnNoteDetailsId => 2325;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(ContactorType, nameof(ContactorType), Characteristics.CONTACTR_TYP),
                mapper.String(InterruptingCapacit, nameof(InterruptingCapacit), Characteristics.INTERRUPTING_CAPACITY),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}