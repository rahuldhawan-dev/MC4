using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FuelTankExcelRecord : EquipmentExcelRecordBase<FuelTankExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 221, EQUPMENT_TYPE = 357;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_TNK_FUEL = 872,
                             CONTAINMENT = 1682,
                             DISTANCE_DITCH_WATERWAY = 1688,
                             FIRE_PROTECTED_MIN_UL_2085 = 1687,
                             FIRE_RESISTANT_MIN_UL_2080 = 1685,
                             LEAK_DETECTION = 1683,
                             LOCATION = 1546,
                             OWNED_BY = 1654,
                             SPECIAL_MAINT_NOTES_DIST = 1658,
                             SPILL_AND_OVERFILL_PROTECTION = 1709,
                             TNK_AUTO_REFILL = 894,
                             TNK_DIAMETER = 1657,
                             TNK_MATERIAL = 1533,
                             TNK_PRESSURE_RATING = 1372,
                             TNK_SIDE_LENGTH = 1656,
                             TNK_STATE_INSPECTION_REQ = 960,
                             TNK_VOLUME = 1655,
                             TNK_FUEL_TYP = 1062,
                             NARUC_MAINTENANCE_ACCOUNT = 2225,
                             NARUC_OPERATIONS_ACCOUNT = 2226,
                             UNDERGROUND = 1073;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string Containment { get; set; }
        public string DistancetoDitchof { get; set; }
        public string FireProtectedmin { get; set; }
        public string FireResistantmin { get; set; }
        public string IndoorOutdoor { get; set; }
        public string LeakDetection { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SpillandOverfillP { get; set; }
        public string TankFuel { get; set; }
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
        protected override int EquipmentPurposeId => EQUPMENT_TYPE;
        protected override int NARUCSpecialMtnNotesId => 2471;
        protected override int NARUCSpecialMtnNoteDetailsId => 2472;

        protected override string EquipmentType => "FUEL TANK";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TNK_FUEL),
                mapper.String(Containment, nameof(Containment), Characteristics.CONTAINMENT),
                mapper.Numerical(DistancetoDitchof, nameof(DistancetoDitchof), Characteristics.DISTANCE_DITCH_WATERWAY),
                mapper.String(FireProtectedmin, nameof(FireProtectedmin), Characteristics.FIRE_PROTECTED_MIN_UL_2085),
                mapper.String(FireResistantmin, nameof(FireResistantmin), Characteristics.FIRE_RESISTANT_MIN_UL_2080),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.String(LeakDetection, nameof(LeakDetection), Characteristics.LEAK_DETECTION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpillandOverfillP, nameof(SpillandOverfillP), Characteristics.SPILL_AND_OVERFILL_PROTECTION),
                mapper.DropDown(TankFuel, nameof(TankFuel), Characteristics.TNK_FUEL_TYP),
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