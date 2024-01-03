using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ChemicalTankExcelRecord : EquipmentExcelRecordBase<ChemicalTankExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 220, EQUIPMENT_PURPOSE = 330;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_TNK_CHEM = 1245,
                             LOCATION = 1185,
                             NARUC_MAINTENANCE_ACCOUNT = 2223,
                             NARUC_OPERATIONS_ACCOUNT = 2224,
                             OWNED_BY = 1649,
                             TNK_CHEM_TYPE = 1536,
                             TNK_AUTO_REFILL = 1451,
                             TNK_DIAMETER = 1652,
                             TNK_MATERIAL = 1143,
                             TNK_PRESSURE_RATING = 1516,
                             TNK_SIDE_LENGTH = 1651,
                             TNK_STATE_INSPECTION_REQ = 1056,
                             TNK_VOLUME = 1650,
                             UNDERGROUND = 1091,
                             SPECIAL_MAINT_NOTES_DIST = 1653;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string IndoorOutdoor { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TankChemical { get; set; }
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
        protected override int NARUCSpecialMtnNotesId => 2469;
        protected override int NARUCSpecialMtnNoteDetailsId => 2470;

        protected override string EquipmentType => "CHEMICAL TANK";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TNK_CHEM),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(TankChemical, nameof(TankChemical), Characteristics.TNK_CHEM_TYPE),
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