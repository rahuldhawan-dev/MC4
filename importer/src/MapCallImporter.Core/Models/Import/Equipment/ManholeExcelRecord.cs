using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ManholeExcelRecord : EquipmentExcelRecordBase<ManholeExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 181,
                         EQUIPMENT_PURPOSE = 375;

        public struct Characteristics
        {
            public const int ASSET_LOCATION = 1161,
                             DEPENDENCY_DRIVER_1 = 839,
                             DEPENDENCY_DRIVER_2 = 847,
                             LINED = 1514,
                             LINED_DATE = 855,
                             MATERIAL_OF_CONSTRUCTION_MH = 1507,
                             MH_ADJUSTING_RING_MATL = 1221,
                             MH_CASTING_MATERIAL = 1529,
                             MH_CONE = 1554,
                             MH_CONE_INSERT = 879,
                             MH_CONE_MATERIAL = 1205,
                             MH_COVER_MATERIAL = 1578,
                             MH_COVER_SIZE = 1078,
                             MH_DROP_TP = 1108,
                             MH_HAS_STEPS = 1416,
                             MH_LID_TP = 1027,
                             MH_PIPE_SEAL_TP = 1387,
                             MH_PONDING = 909,
                             MH_SIZE = 1323,
                             MH_STEP_MATERIAL = 875,
                             MH_TROUGH = 963,
                             MH_TYP = 1454,
                             NARUC_MAINTENANCE_ACCOUNT = 2128,
                             NARUC_OPERATIONS_ACCOUNT = 2129,
                             SPECIAL_MAINT_NOTES_MH = 1398,
                             SURFACE_COVER = 1472,
                             SURFACE_COVER_LOC_TP = 1183,
                             WASTE_WATER_TP = 1071;
        }

        #endregion

        #region Properties

        public string AssetLocation { get; set; }
        public string Basin { get; set; }
        public string BenchTrough { get; set; }
        public string ConeConfiguration { get; set; }
        public string ConeInsert { get; set; }
        public string ConeMaterial { get; set; }
        public string CoverLabel { get; set; }
        public string CoverMaterial { get; set; }
        public string CoverSize { get; set; }
        public string DependencyDriver1 { get; set; }
        public string DependencyDriver2 { get; set; }
        public string GEOACCURACYGISDATA { get; set; }
        public string HasSteps { get; set; }
        public string HistoricalID { get; set; }
        public string InstallationWO { get; set; }
        public string LidCoverType { get; set; }
        public string Lined { get; set; }
        public string LinedDate { get; set; }
        public string LocnSubjecttoPonding { get; set; }
        public string MHofSteps { get; set; }
        public string MHAdjustingRingMatl { get; set; }
        public string MHCastingMaterial { get; set; }
        public string MHDepth { get; set; }
        public string MHDropType { get; set; }
        public string MHPipeSealType { get; set; }
        public string ManholeSize { get; set; }
        public string ManholeType { get; set; }
        public string MapPage { get; set; }
        public string MaterialofConstruction { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StepMaterial { get; set; }
        public string SubBasin { get; set; }
        public string SurfaceCover { get; set; }
        public string SurfaceCoverLocType { get; set; }
        public string WasteWaterType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "MANHOLE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2394;
        protected override int NARUCSpecialMtnNoteDetailsId => 2395;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(AssetLocation, nameof(AssetLocation), Characteristics.ASSET_LOCATION),
                mapper.DropDown(BenchTrough, nameof(BenchTrough), Characteristics.MH_TROUGH),
                mapper.DropDown(ConeInsert, nameof(ConeInsert), Characteristics.MH_CONE_INSERT),
                mapper.DropDown(ConeMaterial, nameof(ConeMaterial), Characteristics.MH_CONE_MATERIAL),
                mapper.DropDown(CoverMaterial, nameof(CoverMaterial), Characteristics.MH_COVER_MATERIAL),
                mapper.DropDown(CoverSize, nameof(CoverSize), Characteristics.MH_COVER_SIZE),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.String(DependencyDriver2, nameof(DependencyDriver2), Characteristics.DEPENDENCY_DRIVER_2),
                mapper.DropDown(HasSteps, nameof(HasSteps), Characteristics.MH_HAS_STEPS),
                mapper.DropDown(LidCoverType, nameof(LidCoverType), Characteristics.MH_LID_TP),
                mapper.DropDown(Lined, nameof(Lined), Characteristics.LINED),
                mapper.Date(LinedDate, nameof(LinedDate), Characteristics.LINED_DATE),
                mapper.DropDown(LocnSubjecttoPonding, nameof(LocnSubjecttoPonding), Characteristics.MH_PONDING),
                mapper.DropDown(MHAdjustingRingMatl, nameof(MHAdjustingRingMatl), Characteristics.MH_ADJUSTING_RING_MATL),
                mapper.DropDown(MHCastingMaterial, nameof(MHCastingMaterial), Characteristics.MH_CASTING_MATERIAL),
                mapper.DropDown(MHDropType, nameof(MHDropType), Characteristics.MH_DROP_TP),
                mapper.DropDown(MHPipeSealType, nameof(MHPipeSealType), Characteristics.MH_PIPE_SEAL_TP),
                mapper.DropDown(ManholeSize, nameof(ManholeSize), Characteristics.MH_SIZE),
                mapper.DropDown(ManholeType, nameof(ManholeType), Characteristics.MH_TYP),
                mapper.DropDown(MaterialofConstruction, nameof(MaterialofConstruction), Characteristics.MATERIAL_OF_CONSTRUCTION_MH),
                mapper.DropDown(StepMaterial, nameof(StepMaterial), Characteristics.MH_STEP_MATERIAL),
                mapper.DropDown(SurfaceCover, nameof(SurfaceCover), Characteristics.SURFACE_COVER),
                mapper.DropDown(SurfaceCoverLocType, nameof(SurfaceCoverLocType), Characteristics.SURFACE_COVER_LOC_TP),
                mapper.DropDown(WasteWaterType, nameof(WasteWaterType), Characteristics.WASTE_WATER_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}