using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class GravitySewerMainExcelRecord : EquipmentExcelRecordBase<GravitySewerMainExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 164,
                         EQUIPMENT_PURPOSE = 360;

        public struct Characteristics
        {
            public const int ASSET_LOCATION = 1074,
                             DEPENDENCY_DRIVER_1 = 842,
                             EAM_PIPE_SIZE = 986,
                             FLOW_DIRECTION = 1441,
                             GMAIN_TYP = 1089,
                             LINED = 932,
                             LINED_DATE = 854,
                             LINED_MATERIAL = 1220,
                             NARUC_MAINTENANCE_ACCOUNT = 2094,
                             NARUC_OPERATIONS_ACCOUNT = 2095,
                             PIPE_MATERIAL = 1388,
                             SPECIAL_MAINT_NOTES_GM = 898,
                             SURFACE_COVER_LOC_TP = 1564,
                             WASTE_WATER_TP = 1239;
        }

        #endregion

        #region Properties

        public string GravityMainType { get; set; }
        public string OwnedBy { get; set; }
        public string UpstreamID { get; set; }
        public string DownstreamID { get; set; }
        public string WasteWaterType { get; set; }
        public string Basin { get; set; }
        public string SubBasin { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDetails { get; set; }
        public string DependencyDriver1 { get; set; }
        public string MapPage { get; set; }
        public string BookPage { get; set; }
        public string PipeChannelSize { get; set; }
        public string PipeMaterial { get; set; }
        public string Lined { get; set; }
        public string LinedDate { get; set; }
        public string LinedMaterial { get; set; }
        public string AssetLocation { get; set; }
        public string SurfaceCoverLocType { get; set; }
        public string FlowDirection { get; set; }
        public string InstallationWO { get; set; }
        public string InspectionFrequency { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "GRAVITY SEWER MAIN";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2360;
        protected override int NARUCSpecialMtnNoteDetailsId => 2361;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(GravityMainType, nameof(GravityMainType), Characteristics.GMAIN_TYP),
                mapper.DropDown(WasteWaterType, nameof(WasteWaterType), Characteristics.WASTE_WATER_TP),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_GM),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.DropDown(Lined, nameof(Lined), Characteristics.LINED),
                mapper.Date(LinedDate, nameof(LinedDate), Characteristics.LINED_DATE),
                mapper.DropDown(LinedMaterial, nameof(LinedMaterial), Characteristics.LINED_MATERIAL),
                mapper.DropDown(AssetLocation, nameof(AssetLocation), Characteristics.ASSET_LOCATION),
                mapper.DropDown(SurfaceCoverLocType, nameof(SurfaceCoverLocType), Characteristics.SURFACE_COVER_LOC_TP),
                mapper.DropDown(FlowDirection, nameof(FlowDirection), Characteristics.FLOW_DIRECTION),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}