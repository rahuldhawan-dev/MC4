using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class VOCStripperExcelRecord : EquipmentExcelRecordBase<VOCStripperExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 233, EQUIPMENT_PURPOSE = 426;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_TRT_STRP = 1413,
                             BACKWASH_RATE = 2006,
                             FLOW_NORMAL_RANGE = 2005,
                             LOCATION = 1202,
                             MATERIAL_OF_CONSTRUCTION = 1142,
                             MEDIA_1_DEPTH = 2004,
                             MEDIA_1_TP_TRT_STRP = 1055,
                             MEDIA_2_TP_TRT_STRP = 1140,
                             MEDIA_REGENERATION_REQD = 1032,
                             NARUC_MAINTENANCE_ACCOUNT = 2249,
                             NARUC_OPERATIONS_ACCOUNT = 2250,
                             NARUC_SPECIAL_MAINT_NOTE_DETAILS = 2496,
                             NARUC_SPECIAL_MAINT_NOTES = 2495,
                             OWNED_BY = 2002,
                             SPECIAL_MAINT_NOTES = 2007,
                             SURFACE_AREA = 2003,
                             TRT_STRP_TYP = 1318,
                             WASH_TP = 1325;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BackwashRategpms { get; set; }
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
        public string StripperType { get; set; }
        public string SurfaceAreasqft { get; set; }
        public string WashType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2495;
        protected override int NARUCSpecialMtnNoteDetailsId => 2496;

        protected override string EquipmentType => "VOC STRIPPER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TRT_STRP),
                mapper.String(BackwashRategpms, nameof(BackwashRategpms), Characteristics.BACKWASH_RATE),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.String(Media1Depth, nameof(Media1Depth), Characteristics.MEDIA_1_DEPTH),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION),
                mapper.DropDown(Media1Type, nameof(Media1Type), Characteristics.MEDIA_1_TP_TRT_STRP),
                mapper.DropDown(Media2Type, nameof(Media2Type), Characteristics.MEDIA_2_TP_TRT_STRP),
                mapper.DropDown(MediaRegenerationR, nameof(MediaRegenerationR), Characteristics.MEDIA_REGENERATION_REQD),
                mapper.DropDown(StripperType, nameof(StripperType), Characteristics.TRT_STRP_TYP),
                mapper.DropDown(WashType, nameof(WashType), Characteristics.WASH_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SurfaceAreasqft, nameof(SurfaceAreasqft), Characteristics.SURFACE_AREA),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
