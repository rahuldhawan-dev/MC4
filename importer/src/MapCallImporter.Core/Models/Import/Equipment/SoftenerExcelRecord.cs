using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class SoftenerExcelRecord : EquipmentExcelRecordBase<SoftenerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 232,
                         EQUIPMENT_PURPOSE = 416;

        public struct Characteristics
        {
            public const int APPLICATION_TRT_SOFT = 1209,
                             BACKWASH_RATE = 1840,
                             FLOW_NORMAL_RANGE = 1839,
                             LOCATION = 1188,
                             MATERIAL_OF_CONSTRUCTION = 867,
                             MEDIA_1_DEPTH = 1837,
                             MEDIA_1_TP_TRT_SOFT = 1515,
                             MEDIA_2_DEPTH = 1838,
                             MEDIA_2_TP_TRT_SOFT = 1228,
                             MEDIA_REGENERATION_REQD = 1315,
                             NARUC_MAINTENANCE_ACCOUNT = 2247,
                             NARUC_OPERATIONS_ACCOUNT = 2248,
                             OWNED_BY = 1835,
                             SPECIAL_MAINT_NOTES = 1841,
                             SURFACE_AREA_SQFT = 1836,
                             TRT_SOFT_TYP = 1236,
                             WASH_TP = 946;
        }

        #endregion

        #region Properties

public string Application {get;set;}
public string BackwashRategpms {get;set;}
public string FlowNormalRange {get;set;}
public string IndoorOutdoor {get;set;}
public string MaterialofConstruc {get;set;}
public string Media1Depth {get;set;}
public string Media1Type {get;set;}
public string Media2Depth {get;set;}
public string Media2Type {get;set;}
public string MediaRegenerationR {get;set;}
public string OwnedBy {get;set;}
public string SoftenerType {get;set;}
public string SpecialMtnNote {get;set;}
public string SpecialMtnNoteDet {get;set;}
public string SurfaceAreasqft {get;set;}
public string WashType {get;set;}
public string NARUCMaintenanceAc {get;set;}
public string NARUCOperationsAcc {get;set;}

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "SOFTENER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2493;
        protected override int NARUCSpecialMtnNoteDetailsId => 2494;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_TRT_SOFT),
                mapper.String(BackwashRategpms, nameof(BackwashRategpms), Characteristics.BACKWASH_RATE),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION),
                mapper.String(Media1Depth, nameof(Media1Depth), Characteristics.MEDIA_1_DEPTH),
                mapper.DropDown(Media1Type, nameof(Media1Type), Characteristics.MEDIA_1_TP_TRT_SOFT),
                mapper.String(Media2Depth, nameof(Media2Depth), Characteristics.MEDIA_2_DEPTH),
                mapper.DropDown(Media2Type, nameof(Media2Type), Characteristics.MEDIA_2_TP_TRT_SOFT),
                mapper.DropDown(MediaRegenerationR, nameof(MediaRegenerationR), Characteristics.MEDIA_REGENERATION_REQD),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(SoftenerType, nameof(SoftenerType), Characteristics.TRT_SOFT_TYP),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(SurfaceAreasqft, nameof(SurfaceAreasqft), Characteristics.SURFACE_AREA_SQFT),
                mapper.DropDown(WashType, nameof(WashType), Characteristics.WASH_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}