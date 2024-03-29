﻿using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class WasteTankExcelRecord : EquipmentExcelRecordBase<WasteTankExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 225,
                         EQUIPMENT_PURPOSE = 427;

        public struct Characteristics
        {
            public const int APPLICATION_TNK_WSTE = 1497,
                             LOCATION = 1216,
                             NARUC_MAINTENANCE_ACCOUNT = 2233,
                             NARUC_OPERATIONS_ACCOUNT = 2234,
                             OWNED_BY = 1674,
                             SPECIAL_MAINT_NOTES_DIST = 1678,
                             TNK_AUTO_REFILL = 1115,
                             TNK_DIAMETER = 1677,
                             TNK_MATERIAL = 1116,
                             TNK_PRESSURE_RATING = 1080,
                             TNK_SIDE_LENGTH = 1676,
                             TNK_STATE_INSPECTION_REQ = 1357,
                             TNK_VOLUME = 1675,
                             TNK_WSTE_TYP = 1308,
                             UNDERGROUND = 1175;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string IndoorOutdoor { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TankWaste { get; set; }
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

        protected override string EquipmentType => "WASTE TANK";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2479;
        protected override int NARUCSpecialMtnNoteDetailsId => 2480;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TNK_WSTE),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.DropDown(TankWaste, nameof(TankWaste), Characteristics.TNK_WSTE_TYP),
                mapper.DropDown(TnkAutoRefill, nameof(TnkAutoRefill), Characteristics.TNK_AUTO_REFILL),
                mapper.Numerical(TnkDiameter, nameof(TnkDiameter), Characteristics.TNK_DIAMETER),
                mapper.DropDown(TnkMaterial, nameof(TnkMaterial), Characteristics.TNK_MATERIAL),
                mapper.DropDown(TnkPressureRating, nameof(TnkPressureRating), Characteristics.TNK_PRESSURE_RATING),
                mapper.Numerical(TnkSideLengthft, nameof(TnkSideLengthft), Characteristics.TNK_SIDE_LENGTH),
                mapper.DropDown(TnkStateInspection, nameof(TnkStateInspection), Characteristics.TNK_STATE_INSPECTION_REQ),
                mapper.Numerical(TnkVolumegal, nameof(TnkVolumegal), Characteristics.TNK_VOLUME),
                mapper.DropDown(Underground, nameof(Underground), Characteristics.UNDERGROUND),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}