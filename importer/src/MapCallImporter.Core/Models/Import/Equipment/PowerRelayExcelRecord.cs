using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerRelayExcelRecord : EquipmentExcelRecordBase<PowerRelayExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 206,
                         EQUIPMENT_PURPOSE = 396;

        public struct Characteristics
        {
            public const int AMP_RATING = 809,
                             BREAKER_NAME = 1520,
                             NARUC_MAINTENANCE_ACCOUNT = 2181,
                             NARUC_OPERATIONS_ACCOUNT = 2182,
                             OWNED_BY = 1966,
                             PHASE_ID = 1557,
                             PWRRELAY_TYP = 1449,
                             STYLE_NUMBER = 1967,
                             VOLT_RATING = 1429;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string BreakerName { get; set; }
        public string OwnedBy { get; set; }
        public string PhaseID { get; set; }
        public string PowerRelayType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StyleNumber { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "POWER RELAY";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2442;
        protected override int NARUCSpecialMtnNoteDetailsId => 2443;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(BreakerName, nameof(BreakerName), Characteristics.BREAKER_NAME),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PhaseID, nameof(PhaseID), Characteristics.PHASE_ID),
                mapper.DropDown(PowerRelayType, nameof(PowerRelayType), Characteristics.PWRRELAY_TYP),
                mapper.String(StyleNumber, nameof(StyleNumber), Characteristics.STYLE_NUMBER),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}