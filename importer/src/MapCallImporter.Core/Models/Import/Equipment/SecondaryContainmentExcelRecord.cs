using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class SecondaryContainmentExcelRecord : EquipmentExcelRecordBase<SecondaryContainmentExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 147, EQUIPMENT_PURPOSE = 413;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_CONTAIN = 1326,
                             CONTAIN_TYP = 1286,
                             LOCATION = 1291,
                             OWNED_BY = 1886,
                             TNK_STATE_INSPECTION_REQ = 1406,
                             TNK_VOLUME_GAL = 1887,
                             UNDERGROUND = 1097,
                             NARUC_MAINTENANCE_ACCOUNT = 2060,
                             NARUC_OPERATIONS_ACCOUNT = 2061;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string ContainmentType { get; set; }
        public string IndoorOutdoor { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TnkStateInspection { get; set; }
        public string TnkVolumegal { get; set; }
        public string Underground { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2326;
        protected override int NARUCSpecialMtnNoteDetailsId => 2327;

        protected override string EquipmentType => "SECONDARY CONTAINMENT";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CONTAIN),
                mapper.DropDown(ContainmentType, nameof(ContainmentType), Characteristics.CONTAIN_TYP),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(TnkStateInspection, nameof(TnkStateInspection), Characteristics.TNK_STATE_INSPECTION_REQ),
                mapper.String(TnkVolumegal, nameof(TnkVolumegal), Characteristics.TNK_VOLUME_GAL),
                mapper.DropDown(Underground, nameof(Underground), Characteristics.UNDERGROUND),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}