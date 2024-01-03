using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ClarifierExcelRecord : EquipmentExcelRecordBase<ClarifierExcelRecord>
    {
        public const int EQUIPMENT_TYPE = 229, EQUIPMENT_PURPOSE = 331;

        public struct Characteristics
        {
            public const int APPLICATION = 1843,
                             AUTO_REMOVAL = 1528,
                             CONTACT_TIME_MINUTES = 1846,
                             FLOW_NORMAL_RANGE = 1845,
                             LOCATION = 1485,
                             MATERIAL_OF_CONSTRUCTION = 1408,
                             NARUC_MAINTENANCE_ACCOUNT = 2241,
                             NARUC_OPERATIONS_ACCOUNT = 2242,
                             OWNED_BY = 1842,
                             SPECIAL_MAINT_NOTES = 1847,
                             SURFACE_AREA_SQFT = 1844,
                             TRT_CLAR_TYP = 1317;
        }

        #region Properties

        public string Application { get; set; }
        public string AutoSludgeRemoval { get; set; }
        public string ClarifierType { get; set; }
        public string ContactTimeminute { get; set; }
        public string FlowNormalRange { get; set; }
        public string IndoorOutdoor { get; set; }
        public string MaterialofConstruc { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SurfaceAreasqft { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2487;
        protected override int NARUCSpecialMtnNoteDetailsId => 2488;

        protected override string EquipmentType => "CLARIFIER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ClarifierType, nameof(ClarifierType), Characteristics.TRT_CLAR_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(Application, nameof(Application), Characteristics.APPLICATION),
                mapper.String(SurfaceAreasqft, nameof(SurfaceAreasqft), Characteristics.SURFACE_AREA_SQFT),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.String(ContactTimeminute, nameof(ContactTimeminute), Characteristics.CONTACT_TIME_MINUTES),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION),
                mapper.DropDown(AutoSludgeRemoval, nameof(AutoSludgeRemoval), Characteristics.AUTO_REMOVAL),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}