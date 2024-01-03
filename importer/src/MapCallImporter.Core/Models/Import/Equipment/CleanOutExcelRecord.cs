using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class CleanOutExcelRecord : EquipmentExcelRecordBase<CleanOutExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 137,
                         EQUIPMENT_PURPOSE = 332;


        public struct Characteristics
        {
            public const int ASSET_LOCATION = 1345,
                             CO_FITTING_TP = 1279,
                             CO_SIZE = 975,
                             CO_SWEEP_TP = 1509,
                             CO_TYP = 1172,
                             DEPENDENCY_DRIVER_1 = 838,
                             DEPENDENCY_DRIVER_2 = 848,
                             MATERIAL_OF_CONSTRUCTION_CO = 1540,
                             NARUC_MAINTENANCE_ACCOUNT = 2040,
                             NARUC_OPERATIONS_ACCOUNT = 2041,
                             SPECIAL_MAINT_NOTES_CO = 1218,
                             SURFACE_COVER = 1256,
                             SURFACE_COVER_LOC_TP = 1261,
                             WASTE_WATER_TP = 1575;
        }


        #endregion

        #region Properties

        public string CleanoutType { get; set; }
        public string OwnedBy { get; set; }
        public string WasteWaterType { get; set; }
        public string Basin { get; set; }
        public string SubBasin { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDetail { get; set; }
        public string DependencyDriver1 { get; set; }
        public string DependencyDriver2 { get; set; }
        public string CleanoutSize { get; set; }
        public string MaterialofConstruction { get; set; }
        public string MapPage { get; set; }
        public string SurfaceCover { get; set; }
        public string SurfaceCoverLocType { get; set; }
        public string AssetLocation { get; set; }
        public string InstallationWO { get; set; }
        public string COFittingType { get; set; }
        public string COSweepType { get; set; }
        public string HistoricalID { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CLEAN OUT";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2306;
        protected override int NARUCSpecialMtnNoteDetailsId => 2307;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(CleanoutType, nameof(CleanoutType), Characteristics.CO_TYP),
                mapper.DropDown(WasteWaterType, nameof(WasteWaterType), Characteristics.WASTE_WATER_TP),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_CO),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.String(DependencyDriver2, nameof(DependencyDriver2), Characteristics.DEPENDENCY_DRIVER_2),
                mapper.DropDown(CleanoutSize, nameof(CleanoutSize), Characteristics.CO_SIZE),
                mapper.DropDown(MaterialofConstruction, nameof(MaterialofConstruction), Characteristics.MATERIAL_OF_CONSTRUCTION_CO),
                mapper.DropDown(SurfaceCover, nameof(SurfaceCover), Characteristics.SURFACE_COVER),
                mapper.DropDown(SurfaceCoverLocType, nameof(SurfaceCoverLocType), Characteristics.SURFACE_COVER_LOC_TP),
                mapper.DropDown(AssetLocation, nameof(AssetLocation), Characteristics.ASSET_LOCATION),
                mapper.DropDown(COFittingType, nameof(COFittingType), Characteristics.CO_FITTING_TP),
                mapper.DropDown(COSweepType, nameof(COSweepType), Characteristics.CO_SWEEP_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}