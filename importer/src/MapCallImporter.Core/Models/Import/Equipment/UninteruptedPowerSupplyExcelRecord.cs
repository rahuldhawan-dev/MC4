using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class UninteruptedPowerSupplyExcelRecord : EquipmentExcelRecordBase<UninteruptedPowerSupplyExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 235,
                         EQUIPMENT_PURPOSE = 422;

        public struct Characteristics
        {
            public const int AMP_RATING = 820,
                             NARUC_MAINTENANCE_ACCOUNT = 2253,
                             NARUC_OPERATIONS_ACCOUNT = 2254,
                             OWNED_BY = 2251,
                             SPECIAL_MAINT_NOTE = 2252,
                             UPS_TYP = 1329,
                             VOLT_RATING = 906;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string UPSType { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "UNINTERUPTED POWER SUPPLY";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2499;
        protected override int NARUCSpecialMtnNoteDetailsId => 2500;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTE),
                mapper.DropDown(UPSType, nameof(UPSType), Characteristics.UPS_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}