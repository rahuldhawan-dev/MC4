using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class GearboxExcelRecord : EquipmentExcelRecordBase<GearboxExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 162,
                         EQUIPMENT_PURPOSE = 359;

        public struct Characteristics
        {
            public const int APPLICATION_GEARBOX = 1051,
                             GEAR_RATIO = 1778,
                             GEARBOX_TYP = 1026,
                             NARUC_MAINTENANCE_ACCOUNT = 2090,
                             NARUC_OPERATIONS_ACCOUNT = 2091,
                             OIL_CAPACITY_GAL = 1779,
                             OIL_TYPE = 1780,
                             OWNED_BY = 1776,
                             RPM_OPERATING = 1777,
                             SPECIAL_MAINT_NOTES = 1781;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string GearRatio { get; set; }
        public string GearboxType { get; set; }
        public string OilCapacitygal { get; set; }
        public string OilType { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "GEARBOX";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2356;
        protected override int NARUCSpecialMtnNoteDetailsId => 2357;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_GEARBOX),
                mapper.String(GearRatio, nameof(GearRatio), Characteristics.GEAR_RATIO),
                mapper.DropDown(GearboxType, nameof(GearboxType), Characteristics.GEARBOX_TYP),
                mapper.String(OilCapacitygal, nameof(OilCapacitygal), Characteristics.OIL_CAPACITY_GAL),
                mapper.String(OilType, nameof(OilType), Characteristics.OIL_TYPE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}