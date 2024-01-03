using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class BlowerExcelRecord : EquipmentExcelRecordBase<BlowerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 125, EQUIPMENT_PURPOSE = 320;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_BLWR = 1407,
                             BHP_RATING = 831,
                             BLWR_TYP = 1004,
                             CAPACITY_RATING = 1704,
                             CAPACITY_UOM_BLWR = 835,
                             DRIVE_TP = 1165,
                             MAX_PRESSURE = 1706,
                             NARUC_MAINTENANCE_ACCOUNT = 2016,
                             NARUC_OPERATIONS_ACCOUNT = 2017,
                             OWNED_BY = 1703,
                             RPM_OPERATING = 1705,
                             SPECIAL_MAINT_NOTES = 1707;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BHPRating { get; set; }
        public string BlowerType { get; set; }
        public string CapacityRating { get; set; }
        public string CapacityUOM { get; set; }
        public string DriveType { get; set; }
        public string MaxPressure { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2282;
        protected override int NARUCSpecialMtnNoteDetailsId => 2283;

        protected override string EquipmentType => "BLOWER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_BLWR),
                mapper.Numerical(BHPRating, nameof(BHPRating), Characteristics.BHP_RATING),
                mapper.DropDown(BlowerType, nameof(BlowerType), Characteristics.BLWR_TYP),
                mapper.Numerical(CapacityRating, nameof(CapacityRating), Characteristics.CAPACITY_RATING),
                mapper.String(CapacityUOM, nameof(CapacityUOM), Characteristics.CAPACITY_UOM_BLWR),
                mapper.DropDown(DriveType, nameof(DriveType), Characteristics.DRIVE_TP),
                mapper.Numerical(MaxPressure, nameof(MaxPressure), Characteristics.MAX_PRESSURE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.Numerical(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
