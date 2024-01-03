using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class StreetValveExcelRecord : EquipmentExcelRecordBase<StreetValveExcelRecord>
    {
        public const int EQUIPMENT_TYPE = 218, EQUIPMENT_PURPOSE = 417;

        public struct Characteristics
        {
            public const int ACCESS_TP = 1162,
                             ACTUATOR_TP = 1501,
                             APPLICATION_SVLV = 921,
                             BOOK_PAGE = 1992,
                             BYPASS_VALVE = 1037,
                             DEPENDENCY_DRIVER_1 = 843,
                             DEPENDENCY_DRIVER_2 = 844,
                             EAM_PIPE_SIZE = 1381,
                             GEAR_TP = 1040,
                             GEOACCURACY_GIS_DATASOURCETYPE = 2001,
                             HISTORICAL_ID = 2000,
                             INSTALLATION_WO = 1998,
                             JOINT_TP = 1285,
                             MAP_PAGE = 1991,
                             NARUC_MAINTENANCE_ACCOUNT = 2209,
                             NARUC_OPERATIONS_ACCOUNT = 2210,
                             NORMAL_POSITION = 1146,
                             NORMAL_SYS_PRESSURE = 856,
                             NUMBER_OF_TURNS = 861,
                             ON_SCADA = 1047,
                             OPEN_DIRECTION = 876,
                             OPERATING_NUT_TP = 1459,
                             OWNED_BY = 1985,
                             PIPE_CHANNEL_SIZE = 1997,
                             PIPE_MATERIAL = 1460,
                             PRESSURE_CLASS = 886,
                             PRESSURE_ZONE = 1989,
                             PRESSURE_ZONE_HGL = 1990,
                             SKETCH = 1999,
                             SPECIAL_MAINT_NOTES_DETAILS = 1986,
                             SPECIAL_MAINT_NOTES_DIST = 904,
                             STREET_VALVE_TYPE = 1987,
                             SUBDIVISION = 1988,
                             SURFACE_COVER = 930,
                             SURFACE_COVER_LOC_TP = 1102,
                             SVLV_TYP = 1349,
                             TORQUE_LIMIT = 1993,
                             VALVE_BOX_MARKING = 1996,
                             VLV_DEPTH_TOP_OF_MAIN = 1994,
                             VLV_OPER_NUT_SIZE = 1003,
                             VLV_SEAT_TP = 1136,
                             VLV_SPECIAL_V_BOX_MARKING = 1476,
                             VLV_TOP_VALVE_NUT_DEPTH = 1995,
                             VLV_VALVE_SIZE = 1180,
                             VLV_VALVE_TP = 1577;
        }

        #region Properties

        public string ActuatorType { get; set; }
        public string Application { get; set; }
        public string BookPage { get; set; }
        public string BypassValve { get; set; }
        public string DependencyDriver1 { get; set; }
        public string DependencyDriver2 { get; set; }
        public string GEOACCURACYGISDATA { get; set; }
        public string GearType { get; set; }
        public string HistoricalID { get; set; }
        public string InstallationWO { get; set; }
        public string JointType { get; set; }
        public string MapPage { get; set; }
        public string NormalPosition { get; set; }
        public string NormalSysPressure { get; set; }
        public string NumberofTurns { get; set; }
        public string OnSCADA { get; set; }
        public string OpenDirection { get; set; }
        public string OperatingNutType { get; set; }
        public string OwnedBy { get; set; }
        public string PipeChannelSize { get; set; }
        public string PipeMaterial { get; set; }
        public string PressureClass { get; set; }
        public string PressureZone { get; set; }
        public string PressureZoneHGL { get; set; }
        public string Sketch { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StreetValveType { get; set; }
        public string Subdivision { get; set; }
        public string SurfaceCover { get; set; }
        public string SurfaceCoverLocTy { get; set; }
        public string TorqueLimit { get; set; }
        public string VLVAccessType { get; set; }
        public string VlvDepthtoTopof { get; set; }
        public string VlvOperNutSize { get; set; }
        public string VlvSeatType { get; set; }
        public string VlvSpecialVBoxMa { get; set; }
        public string VlvTopValveNutDe { get; set; }
        public string VlvValveBoxMarkin { get; set; }
        public string VlvValveSize { get; set; }
        public string VlvValveType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2465;
        protected override int NARUCSpecialMtnNoteDetailsId => 2466;

        protected override string EquipmentType => "STREET VALVE";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ActuatorType, nameof(ActuatorType), Characteristics.ACTUATOR_TP),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_SVLV),
                mapper.String(BookPage, nameof(BookPage), Characteristics.BOOK_PAGE),
                mapper.DropDown(BypassValve, nameof(BypassValve), Characteristics.BYPASS_VALVE),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.String(DependencyDriver2, nameof(DependencyDriver2), Characteristics.DEPENDENCY_DRIVER_2),
                mapper.DropDown(GearType, nameof(GearType), Characteristics.GEAR_TP),
                mapper.String(GEOACCURACYGISDATA, nameof(GEOACCURACYGISDATA), Characteristics.GEOACCURACY_GIS_DATASOURCETYPE),
                mapper.String(HistoricalID, nameof(HistoricalID), Characteristics.HISTORICAL_ID),
                mapper.String(InstallationWO, nameof(InstallationWO), Characteristics.INSTALLATION_WO),
                mapper.DropDown(JointType, nameof(JointType), Characteristics.JOINT_TP),
                mapper.String(MapPage, nameof(MapPage), Characteristics.MAP_PAGE),
                mapper.DropDown(NormalPosition, nameof(NormalPosition), Characteristics.NORMAL_POSITION),
                mapper.Numerical(NormalSysPressure, nameof(NormalSysPressure), Characteristics.NORMAL_SYS_PRESSURE),
                mapper.Numerical(NumberofTurns, nameof(NumberofTurns), Characteristics.NUMBER_OF_TURNS),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OpenDirection, nameof(OpenDirection), Characteristics.OPEN_DIRECTION),
                mapper.DropDown(OperatingNutType, nameof(OperatingNutType), Characteristics.OPERATING_NUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(PipeChannelSize, nameof(PipeChannelSize), Characteristics.PIPE_CHANNEL_SIZE),
                mapper.DropDown(PipeMaterial, nameof(PipeMaterial), Characteristics.PIPE_MATERIAL),
                mapper.DropDown(PressureClass, nameof(PressureClass), Characteristics.PRESSURE_CLASS),
                mapper.String(PressureZone, nameof(PressureZone), Characteristics.PRESSURE_ZONE),
                mapper.String(PressureZoneHGL, nameof(PressureZoneHGL), Characteristics.PRESSURE_ZONE_HGL),
                mapper.String(Sketch, nameof(Sketch), Characteristics.SKETCH),
                mapper.DropDown(StreetValveType, nameof(StreetValveType), Characteristics.SVLV_TYP),
                mapper.String(Subdivision, nameof(Subdivision), Characteristics.SUBDIVISION),
                mapper.DropDown(SurfaceCover, nameof(SurfaceCover), Characteristics.SURFACE_COVER),
                mapper.DropDown(SurfaceCoverLocTy, nameof(SurfaceCoverLocTy), Characteristics.SURFACE_COVER_LOC_TP),
                mapper.String(TorqueLimit, nameof(TorqueLimit), Characteristics.TORQUE_LIMIT),
                mapper.DropDown(VLVAccessType, nameof(VLVAccessType), Characteristics.ACCESS_TP),
                mapper.String(VlvDepthtoTopof, nameof(VlvDepthtoTopof), Characteristics.VLV_DEPTH_TOP_OF_MAIN),
                mapper.DropDown(VlvOperNutSize, nameof(VlvOperNutSize), Characteristics.VLV_OPER_NUT_SIZE),
                mapper.DropDown(VlvSeatType, nameof(VlvSeatType), Characteristics.VLV_SEAT_TP),
                mapper.String(VlvTopValveNutDe, nameof(VlvTopValveNutDe), Characteristics.VLV_TOP_VALVE_NUT_DEPTH),
                mapper.DropDown(VlvSpecialVBoxMa, nameof(VlvSpecialVBoxMa), Characteristics.VLV_SPECIAL_V_BOX_MARKING),
                mapper.String(VlvValveBoxMarkin, nameof(VlvValveBoxMarkin), Characteristics.VALVE_BOX_MARKING),
                mapper.DropDown(VlvValveSize, nameof(VlvValveSize), Characteristics.VLV_VALVE_SIZE),
                mapper.DropDown(VlvValveType, nameof(VlvValveType), Characteristics.VLV_VALVE_TP),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(SpecialMtnNoteDet, nameof(SpecialMtnNoteDet), Characteristics.SPECIAL_MAINT_NOTES_DETAILS),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}