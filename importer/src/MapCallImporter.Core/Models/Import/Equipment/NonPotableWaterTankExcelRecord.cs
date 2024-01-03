using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class NonPotableWaterTankExcelRecord : EquipmentExcelRecordBase<NonPotableWaterTankExcelRecord>
    {
        #region Constants

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_TNK_WNON = 1360,
                             LOCATION = 1090,
                             OWNED_BY = 1664,
                             SPECIAL_MAINT_NOTES_DIST = 1668,
                             TNK_AUTO_REFILL = 1500,
                             TNK_DIAMETER = 1667,
                             TNK_MATERIAL = 1392,
                             TNK_PRESSURE_RATING = 925,
                             TNK_SIDE_LENGTH = 1666,
                             TNK_STATE_INSPECTION_REQ = 1160,
                             TNK_VOLUME = 1665,
                             TNK_WNON_TYP = 1503,
                             NARUC_MAINTENANCE_ACCOUNT = 2229,
                             NARUC_OPERATIONS_ACCOUNT = 2230,
                             UNDERGROUND = 1007;

            #endregion
        }

        public const int EQUIPMENT_TYPE = 223;
        public const int EQUIPMENT_PURPOSE = 383;

        #endregion

        #region Properties

        public string Application { get; set; }
        public string IndoorOutdoor { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TankNonPotableWat { get; set; }
        public string TnkAutoRefill { get; set; }
        public string TnkDiameter { get; set; }
        public string TnkMaterial { get; set; }
        public string TnkPressureRating { get; set; }
        public string TnkSideLengthft { get; set; }
        public string TnkStateInspection { get; set; }
        public string TnkVolumegal { get; set; }
        public string Underground { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2475;
        protected override int NARUCSpecialMtnNoteDetailsId => 2476;

        protected override string EquipmentType => "NON POTABLE WATER TANK";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TNK_WNON),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(TankNonPotableWat, nameof(TankNonPotableWat), Characteristics.TNK_WNON_TYP),
                mapper.DropDown(TnkAutoRefill, nameof(TnkAutoRefill), Characteristics.TNK_AUTO_REFILL),
                mapper.Numerical(TnkDiameter, nameof(TnkDiameter), Characteristics.TNK_DIAMETER),
                mapper.DropDown(TnkMaterial, nameof(TnkMaterial), Characteristics.TNK_MATERIAL),
                mapper.DropDown(TnkPressureRating, nameof(TnkPressureRating), Characteristics.TNK_PRESSURE_RATING),
                mapper.Numerical(TnkSideLengthft, nameof(TnkSideLengthft), Characteristics.TNK_SIDE_LENGTH),
                mapper.DropDown(TnkStateInspection, nameof(TnkStateInspection), Characteristics.TNK_STATE_INSPECTION_REQ),
                mapper.Numerical(TnkVolumegal, nameof(TnkVolumegal), Characteristics.TNK_VOLUME),
                mapper.DropDown(Underground, nameof(Underground), Characteristics.UNDERGROUND),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}