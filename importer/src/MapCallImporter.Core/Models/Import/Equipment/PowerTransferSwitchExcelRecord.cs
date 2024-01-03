using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerTransferSwitchExcelRecord : EquipmentExcelRecordBase<PowerTransferSwitchExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 227, EQUIPMENT_PURPOSE = 398;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 819,
                             TRAN_SW_TYP = 983,
                             VOLT_RATING = 1092,
                             PROGRAMMABLE = 1123,
                             OWNED_BY = 1792,
                             NARUC_MAINTENANCE_ACCOUNT = 2237,
                             NARUC_OPERATIONS_ACCOUNT = 2238,
                             SPECIAL_MAINT_NOTES = 1793;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string OwnedBy { get; set; }
        public string Programmable { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TransferSwitchType { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2483;
        protected override int NARUCSpecialMtnNoteDetailsId => 2484;

        protected override string EquipmentType => "POWER TRANSFER SWITCH";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(Programmable, nameof(Programmable), Characteristics.PROGRAMMABLE),
                mapper.DropDown(TransferSwitchType, nameof(TransferSwitchType), Characteristics.TRAN_SW_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
