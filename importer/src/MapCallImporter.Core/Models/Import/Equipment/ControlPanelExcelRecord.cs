using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ControlPanelExcelRecord : EquipmentExcelRecordBase<ControlPanelExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 135,
                         EQUIPMENT_PURPOSE = 333;

        public struct Characteristics
        {
            public const int AMP_RATING = 816,
                             CNTRLPNL_TYP = 1569,
                             NARUC_MAINTENANCE_ACCOUNT = 2036,
                             NARUC_OPERATIONS_ACCOUNT = 2037,
                             OWNED_BY = 1735,
                             SPECIAL_MAINT_NOTES = 1736,
                             VOLT_RATING = 1103;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string ControlPanelType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CONTROL PANEL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2302;
        protected override int NARUCSpecialMtnNoteDetailsId => 2303;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(ControlPanelType, nameof(ControlPanelType), Characteristics.CNTRLPNL_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}