using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class WaterTreatmentContactorExcelRecord : EquipmentExcelRecordBase<WaterTreatmentContactorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 230,
                         EQUIPMENT_PURPOSE = 430;

        public struct Characteristics
        {
            public const int APPLICATION_TRT_CONT = 1458,
                             BACKWASH_RATE_GPM_SQFT = 1853,
                             FLOW_NORMAL_RANGE = 1852,
                             LOCATION = 1128,
                             MATERIAL_OF_CONSTRUCTION = 1347,
                             MEDIA_1_DEPTH = 1850,
                             MEDIA_1_TP_TRT_CONT = 949,
                             MEDIA_2_DEPTH = 1851,
                             MEDIA_2_TP_TRT_CONT = 937,
                             MEDIA_REGENERATION_REQD = 1212,
                             NARUC_MAINTENANCE_ACCOUNT = 2243,
                             NARUC_OPERATIONS_ACCOUNT = 2244,
                             OWNED_BY = 1848,
                             SURFACE_AREA_SQFT = 1849,
                             TRT_CONT_TYP = 1041,
                             WASH_TP = 1252;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BackwashRategpms { get; set; }
        public string ContactorType { get; set; }
        public string FlowNormalRange { get; set; }
        public string IndoorOutdoor { get; set; }
        public string MaterialofConstruc { get; set; }
        public string Media1Depth { get; set; }
        public string Media1Type { get; set; }
        public string Media2Depth { get; set; }
        public string Media2Type { get; set; }
        public string MediaRegenerationR { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SurfaceAreasqft { get; set; }
        public string WashType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "WATER TREATMENT CONTACTOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2489;
        protected override int NARUCSpecialMtnNoteDetailsId => 2490;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TRT_CONT),
                mapper.String(BackwashRategpms, nameof(BackwashRategpms), Characteristics.BACKWASH_RATE_GPM_SQFT),
                mapper.DropDown(ContactorType, nameof(ContactorType), Characteristics.TRT_CONT_TYP),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION),
                mapper.String(Media1Depth, nameof(Media1Depth), Characteristics.MEDIA_1_DEPTH),
                mapper.DropDown(Media1Type, nameof(Media1Type), Characteristics.MEDIA_1_TP_TRT_CONT),
                mapper.String(Media2Depth, nameof(Media2Depth), Characteristics.MEDIA_2_DEPTH),
                mapper.DropDown(Media2Type, nameof(Media2Type), Characteristics.MEDIA_2_TP_TRT_CONT),
                mapper.DropDown(MediaRegenerationR, nameof(MediaRegenerationR), Characteristics.MEDIA_REGENERATION_REQD),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SurfaceAreasqft, nameof(SurfaceAreasqft), Characteristics.SURFACE_AREA_SQFT),
                mapper.DropDown(WashType, nameof(WashType), Characteristics.WASH_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}