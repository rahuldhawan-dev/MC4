using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ScrubberExcelRecord : EquipmentExcelRecordBase<ScrubberExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 214,
                         EQUIPMENT_PURPOSE = 412;

        public struct Characteristics
        {
            public const int APPLICATION_SCRBBR = 1269,
                             BACK_WASH_RATE_GPM_SQFT = 1815,
                             FLOW_NORMAL_RANGE = 1814,
                             LOCATION = 1095,
                             MATERIAL_OF_CONSTRUCTION = 1022,
                             MEDIA_1_DEPTH = 1812,
                             MEDIA_1_TP_SCRBBR = 1058,
                             MEDIA_2_DEPTH = 1813,
                             MEDIA_2_TP_SCRBBR = 994,
                             MEDIA_REGENERATION_REQD = 1254,
                             NARUC_MAINTENANCE_ACCOUNT = 2201,
                             NARUC_OPERATIONS_ACCOUNT = 2202,
                             OWNED_BY = 1810,
                             SCRBBR_TYP = 1070,
                             SPECIAL_MAINT_NOTES = 1817,
                             SURFACE_AREA_SQFT = 1811,
                             WASH_TP = 1305;
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
        public string ScrubberType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SurfaceAreasqft { get; set; }
        public string WashType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "SCRUBBER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2457;
        protected override int NARUCSpecialMtnNoteDetailsId => 2458;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_SCRBBR),
                mapper.String(BackwashRategpms, nameof(BackwashRategpms), Characteristics.BACK_WASH_RATE_GPM_SQFT),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc),
                    Characteristics.MATERIAL_OF_CONSTRUCTION),
                mapper.String(Media1Depth, nameof(Media1Depth), Characteristics.MEDIA_1_DEPTH),
                mapper.DropDown(Media1Type, nameof(Media1Type), Characteristics.MEDIA_1_TP_SCRBBR),
                mapper.String(Media2Depth, nameof(Media2Depth), Characteristics.MEDIA_2_DEPTH),
                mapper.DropDown(Media2Type, nameof(Media2Type), Characteristics.MEDIA_2_TP_SCRBBR),
                mapper.DropDown(MediaRegenerationR, nameof(MediaRegenerationR),
                    Characteristics.MEDIA_REGENERATION_REQD),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(ScrubberType, nameof(ScrubberType), Characteristics.SCRBBR_TYP),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(SurfaceAreasqft, nameof(SurfaceAreasqft), Characteristics.SURFACE_AREA_SQFT),
                mapper.DropDown(WashType, nameof(WashType), Characteristics.WASH_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc),
                    Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}