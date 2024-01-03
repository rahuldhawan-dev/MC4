using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class VehicleExcelRecord : EquipmentExcelRecordBase<VehicleExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 236,
                         EQUIPMENT_PURPOSE = 425;

        public struct Characteristics
        {
            public const int CAPACITY_RATING = 1855,
                             CAPACITY_UOM = 1044,
                             NARUC_MAINTENANCE_ACCOUNT = 2255,
                             NARUC_OPERATIONS_ACCOUNT = 2256,
                             OWNED_BY = 1854,
                             RETEST_REQUIRED = 1542,
                             SPECIAL_MAINT_NOTES = 1856,
                             VEH_TYP = 883;
        }

        #endregion

        #region Properties

        public string CapacityRating { get; set; }
        public string CapacityUOM { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VehicleType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "VEHICLE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2501;
        protected override int NARUCSpecialMtnNoteDetailsId => 2502;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(CapacityRating, nameof(CapacityRating), Characteristics.CAPACITY_RATING),
                mapper.DropDown(CapacityUOM, nameof(CapacityUOM), Characteristics.CAPACITY_UOM),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.DropDown(VehicleType, nameof(VehicleType), Characteristics.VEH_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}