using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class AdjustableSpeedDriveExcelRecord : EquipmentExcelRecordBase<AdjustableSpeedDriveExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 121,
            EQUIPMENT_PURPOSE = 310;

        public struct Characteristics
        {
            public const int ADJSPD_TYP = 1462,
                             AMP_RATING = 811,
                             FULL_LOAD_AMPS = 1691,
                             HP_RATING = 852,
                             OWNED_BY = 1708,
                             PULSE_TP = 1260,
                             SPECIAL_MAINT_NOTES = 1692,
                             NARUC_MAINTENANCE_ACCOUNT = 2008,
                             NARUC_OPERATIONS_ACCOUNT = 2009,
                             VOLT_RATING = 1461;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string FullLoadAmps { get; set; }
        public string HPRating { get; set; }
        public string OwnedBy { get; set; }
        public string PulseType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SpeedDriveType { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2274;
        protected override int NARUCSpecialMtnNoteDetailsId => 2275;

        protected override string EquipmentType => "ADJUSTABLE SPEED DRIVE";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.Numerical(FullLoadAmps, nameof(FullLoadAmps), Characteristics.FULL_LOAD_AMPS),
                mapper.Numerical(HPRating, nameof(HPRating), Characteristics.HP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PulseType, nameof(PulseType), Characteristics.PULSE_TP),
                mapper.DropDown(SpeedDriveType, nameof(SpeedDriveType), Characteristics.ADJSPD_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
