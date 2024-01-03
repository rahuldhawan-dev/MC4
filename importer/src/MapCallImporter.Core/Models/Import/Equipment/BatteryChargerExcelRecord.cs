using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class BatteryChargerExcelRecord : EquipmentExcelRecordBase<BatteryChargerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 124, EQUIPMENT_PURPOSE = 318;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 812,
                             BATTCHGR_TYP = 1486,
                             OWNED_BY = 1701,
                             VOLT_RATING = 1579,
                             NARUC_MAINTENANCE_ACCOUNT = 2014,
                             NARUC_OPERATIONS_ACCOUNT = 2015,
                             SPECIAL_MAINT_NOTES = 1702;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string BatteryChargerType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2280;
        protected override int NARUCSpecialMtnNoteDetailsId => 2281;

        protected override string EquipmentType => "BATTERY CHARGER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(BatteryChargerType, nameof(BatteryChargerType),
                    Characteristics.BATTCHGR_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}