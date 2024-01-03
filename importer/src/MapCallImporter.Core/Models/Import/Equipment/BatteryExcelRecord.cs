using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class BatteryExcelRecord : EquipmentExcelRecordBase<BatteryExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 123;
        public const int EQUIPMENT_PURPOSE = 317;

        public struct Characteristics
        {
            #region Constants

            public const int BATTERIES_BANK = 1696,
                             AMP_HOURS = 1698,
                             BATT_CELL_TP = 1265,
                             BATT_TYP = 1284,
                             COLD_CRANKING_AMPS = 1697,
                             OWNED_BY = 1695,
                             BATT_VOLT_RATING = 1105,
                             NARUC_MAINTENANCE_ACCOUNT = 2012,
                             NARUC_OPERATIONS_ACCOUNT = 2013,
                             SPECIAL_MAINT_NOTES = 1700;

            #endregion
        }

        #endregion

        #region Properties

        public string BatteriesinBank { get; set; }
        public string AmpHoursea { get; set; }
        public string BatCellType { get; set; }
        public string BatteryType { get; set; }
        public string ColdCrankingAmpse { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2278;
        protected override int NARUCSpecialMtnNoteDetailsId => 2279;

        protected override string EquipmentType => "BATTERY";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(BatteriesinBank, nameof(BatteriesinBank), Characteristics.BATTERIES_BANK),
                mapper.String(AmpHoursea, nameof(AmpHoursea), Characteristics.AMP_HOURS),
                mapper.DropDown(BatCellType, nameof(BatCellType), Characteristics.BATT_CELL_TP),
                mapper.DropDown(BatteryType, nameof(BatteryType), Characteristics.BATT_TYP),
                mapper.String(ColdCrankingAmpse, nameof(ColdCrankingAmpse), Characteristics.COLD_CRANKING_AMPS),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.BATT_VOLT_RATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}