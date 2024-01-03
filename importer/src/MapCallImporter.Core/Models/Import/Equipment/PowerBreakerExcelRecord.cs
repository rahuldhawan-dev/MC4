using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerBreakerExcelRecord : EquipmentExcelRecordBase<PowerBreakerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 200, EQUIPMENT_PURPOSE = 390;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 797,
                             BREAKER_TP = 1117,
                             OWNED_BY = 1956,
                             PWRBRKR_TYP = 924,
                             NARUC_MAINTENANCE_ACCOUNT = 2169,
                             NARUC_OPERATIONS_ACCOUNT = 2170,
                             VOLT_RATING = 1356;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string BreakerType { get; set; }
        public string OwnedBy { get; set; }
        public string PowerBreakerType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2430;
        protected override int NARUCSpecialMtnNoteDetailsId => 2431;

        protected override string EquipmentType => "POWER BREAKER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(BreakerType, nameof(BreakerType), Characteristics.BREAKER_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerBreakerType, nameof(PowerBreakerType), Characteristics.PWRBRKR_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}