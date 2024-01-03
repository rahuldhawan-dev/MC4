using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class MixerExcelRecord : EquipmentExcelRecordBase<MixerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 182,
                         EQUIPMENT_PURPOSE = 376;

        public struct Characteristics
        {
            public const int APPLICATION_MIXR = 1583,
                             BHP_RATING = 834,
                             FLOW_UOM = 1052,
                             MIXR_TYP = 1079,
                             NARUC_MAINTENANCE_ACCOUNT = 2130,
                             NARUC_OPERATIONS_ACCOUNT = 2131,
                             OWNED_BY = 1786,
                             RPM_OPERATING = 1787,
                             SPECIAL_MAINT_NOTES = 1788;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BHPRating { get; set; }
        public string FlowUOM { get; set; }
        public string MixerType { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "MIXER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2396;
        protected override int NARUCSpecialMtnNoteDetailsId => 2397;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_MIXR),
                mapper.Numerical(BHPRating, nameof(BHPRating), Characteristics.BHP_RATING),
                mapper.DropDown(FlowUOM, nameof(FlowUOM), Characteristics.FLOW_UOM),
                mapper.DropDown(MixerType, nameof(MixerType), Characteristics.MIXR_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}