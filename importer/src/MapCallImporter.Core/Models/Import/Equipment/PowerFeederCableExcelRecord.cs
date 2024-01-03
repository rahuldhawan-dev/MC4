using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerFeederCableExcelRecord : EquipmentExcelRecordBase<PowerFeederCableExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 203,
                         EQUIPMENT_PURPOSE = 393;

        public struct Characteristics
        {
            public const int AMP_RATING = 799,
                             NARUC_MAINTENANCE_ACCOUNT = 2175,
                             NARUC_OPERATIONS_ACCOUNT = 2176,
                             OWNED_BY = 1960,
                             PWRFEEDR_TYP = 1012,
                             VOLT_RATING = 1522,
                             WIRE_INSULATION_TP = 919,
                             WIRE_SIZE = 1961;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string OwnedBy { get; set; }
        public string PowerFeederType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string WireInsulationType { get; set; }
        public string WireSize { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "POWER FEEDER CABLE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2436;
        protected override int NARUCSpecialMtnNoteDetailsId => 2437;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerFeederType, nameof(PowerFeederType), Characteristics.PWRFEEDR_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.DropDown(WireInsulationType, nameof(WireInsulationType), Characteristics.WIRE_INSULATION_TP),
                mapper.String(WireSize, nameof(WireSize), Characteristics.WIRE_SIZE),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}