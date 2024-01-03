using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PhaseConverterExcelRecord : EquipmentExcelRecordBase<PhaseConverterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 189,
                         EQUIPMENT_PURPOSE = 387;

        public struct Characteristics
        {
            public const int NARUC_MAINTENANCE_ACCOUNT = 2143,
                             NARUC_OPERATIONS_ACCOUNT = 2144,
                             PHASECON_TYP = 1312;
        }

        #endregion

        #region Properties

        public string PhaseConverterType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "PHASE CONVERTER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2408;
        protected override int NARUCSpecialMtnNoteDetailsId => 2409;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(PhaseConverterType, nameof(PhaseConverterType), Characteristics.PHASECON_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}