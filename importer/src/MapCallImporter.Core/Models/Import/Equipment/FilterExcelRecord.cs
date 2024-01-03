using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FilterExcelRecord : EquipmentExcelRecordBase<FilterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 231;
        public const int EQUIPMENT_PURPOSE = 349;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_TRT_FILT = 1194,
                             BACKWASH_RATE_GPM_SGFT = 1871,
                             FLOW_NORMAL_RANGE = 1870,
                             LOCATION = 1053,
                             MATERIAL_OF_CONSTRUCTION = 1186,
                             MEDIA_1_DEPTH = 1865,
                             MEDIA_1_TP_TRT_FILT = 1203,
                             MEDIA_2_DEPTH = 1866,
                             MEDIA_2_TP_TRT_FILT = 903,
                             MEDIA_3_DEPTH = 1867,
                             MEDIA_3_TP = 1043,
                             MEDIA_4_DEPTH = 1868,
                             MEDIA_4_TP = 1233,
                             MEDIA_5_DEPTH = 1869,
                             MEDIA_5_TP = 1259,
                             MEDIA_REGENERATION_REQD = 1588,
                             OWNED_BY = 1863,
                             NARUC_MAINTENANCE_ACCOUNT = 2245,
                             NARUC_OPERATIONS_ACCOUNT = 2246,
                             SPECIAL_MAINT_NOTES = 1872,
                             SURFACE_AREA_SQFT = 1864,
                             TRT_FILT_TYP = 1434,
                             WASH_TP = 1393;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BackwashRategpms { get; set; }
        public string FilterType { get; set; }
        public string FlowNormalRange { get; set; }
        public string IndoorOutdoor { get; set; }
        public string MaterialofConstruc { get; set; }
        public string Media1Depth { get; set; }
        public string Media1Type { get; set; }
        public string Media2Depth { get; set; }
        public string Media2Type { get; set; }
        public string Media3Depth { get; set; }
        public string Media3Type { get; set; }
        public string Media4Depth { get; set; }
        public string Media4Type { get; set; }
        public string Media5Depth { get; set; }
        public string Media5Type { get; set; }
        public string MediaRegenerationR { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SurfaceAreasqft { get; set; }
        public string WashType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2491;
        protected override int NARUCSpecialMtnNoteDetailsId => 2492;

        protected override string EquipmentType => "FILTER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TRT_FILT),
                mapper.String(BackwashRategpms, nameof(BackwashRategpms), Characteristics.BACKWASH_RATE_GPM_SGFT),
                mapper.DropDown(FilterType, nameof(FilterType), Characteristics.TRT_FILT_TYP),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION),
                mapper.Numerical(Media1Depth, nameof(Media1Depth), Characteristics.MEDIA_1_DEPTH),
                mapper.DropDown(Media1Type, nameof(Media1Type), Characteristics.MEDIA_1_TP_TRT_FILT),
                mapper.Numerical(Media2Depth, nameof(Media2Depth), Characteristics.MEDIA_2_DEPTH),
                mapper.DropDown(Media2Type, nameof(Media2Type), Characteristics.MEDIA_2_TP_TRT_FILT),
                mapper.Numerical(Media3Depth, nameof(Media3Depth), Characteristics.MEDIA_3_DEPTH),
                mapper.DropDown(Media3Type, nameof(Media3Type), Characteristics.MEDIA_3_TP),
                mapper.Numerical(Media4Depth, nameof(Media4Depth), Characteristics.MEDIA_4_DEPTH),
                mapper.DropDown(Media4Type, nameof(Media4Type), Characteristics.MEDIA_4_TP),
                mapper.Numerical(Media5Depth, nameof(Media5Depth), Characteristics.MEDIA_5_DEPTH),
                mapper.DropDown(Media5Type, nameof(Media5Type), Characteristics.MEDIA_5_TP),
                mapper.DropDown(MediaRegenerationR, nameof(MediaRegenerationR), Characteristics.MEDIA_REGENERATION_REQD),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SurfaceAreasqft, nameof(SurfaceAreasqft), Characteristics.SURFACE_AREA_SQFT),
                mapper.DropDown(WashType, nameof(WashType), Characteristics.WASH_TP),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}