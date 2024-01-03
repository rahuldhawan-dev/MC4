using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class BlowOffValveExcelRecord : EquipmentExcelRecordBase<BlowOffValveExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 219,
                         EQUIPMENT_PURPOSE = 319;

        public struct Characteristics
        {
            public const int ACCESS_TP = 1490,
                             ACTUATOR_TP = 1283,
                             APPLICATION_SVLV_BO = 827,
                             BOOK_PAGE = 2220,
                             BYPASS_VALVE = 1231,
                             DEPENDENCY_DRIVER_1 = 841,
                             DEPENDENCY_DRIVER_2 = 845,
                             EAM_PIPE_SIZE = 1565,
                             GEAR_TP = 1344,
                             JOINT_TP = 1036,
                             MAP_PAGE = 2219,
                             NARUC_MAINTENANCE_ACCOUNT = 2213,
                             NARUC_OPERATIONS_ACCOUNT = 2214,
                             NORMAL_POSITION = 1069,
                             NORMAL_SYS_PRESSURE = 858,
                             NUMBER_OF_TURNS = 862,
                             ON_SCADA = 911,
                             OPEN_DIRECTION = 923,
                             OPERATING_NUT_TP = 1437,
                             OWNED_BY = 2211,
                             PIPE_MATERIAL = 1464,
                             PRESSURE_CLASS = 913,
                             PRESSURE_ZONE = 2217,
                             PRESSURE_ZONE_HGL = 2218,
                             SPECIAL_MAINT_NOTE = 2212,
                             SPECIAL_MAINT_NOTE_DETAILS = 2215,
                             SPECIAL_MAINT_NOTES_DIST = 1268,
                             SUBDIVISION = 2216,
                             SURFACE_COVER = 1586,
                             SURFACE_COVER_LOC_TP = 1496,
                             SVLV_BO_TYP = 1029,
                             TORQUE_LIMIT = 2221,
                             VLV_OPER_NUT_SIZE = 1179,
                             VLV_SEAT_TP = 1257,
                             VLV_SPECIAL_V_BOX_MARKING = 887,
                             VLV_VALVE_SIZE = 1150,
                             VLV_VALVE_TP = 1431;
        }

        #endregion

        #region Properties

        public string ActuatorType { get; set; }
        public string Application { get; set; }
        public string BlowoffType { get; set; }
        public string BookPage { get; set; }
        public string BypassValve { get; set; }
        public string DependencyDriver1 { get; set; }
        public string DependencyDriver2 { get; set; }
        public string GEOACCURACYGISDATA { get; set; }
        public string GearType { get; set; }
        public string HistoricalID { get; set; }
        public string InstallationWO {get;set;}
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

        protected override string EquipmentType => "BLOW OFF VALVE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2467;
        protected override int NARUCSpecialMtnNoteDetailsId => 2468;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ActuatorType, nameof(ActuatorType), Characteristics.ACTUATOR_TP),
                mapper.String(Application, nameof(Application), Characteristics.APPLICATION_SVLV_BO),
                mapper.DropDown(BlowoffType, nameof(BlowoffType), Characteristics.SVLV_BO_TYP),
                mapper.String(BookPage, nameof(BookPage), Characteristics.BOOK_PAGE),
                mapper.DropDown(BypassValve, nameof(BypassValve), Characteristics.BYPASS_VALVE),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.String(DependencyDriver2, nameof(DependencyDriver2), Characteristics.DEPENDENCY_DRIVER_2),
                mapper.DropDown(GearType, nameof(GearType), Characteristics.GEAR_TP),
                mapper.DropDown(JointType, nameof(JointType), Characteristics.JOINT_TP),
                mapper.String(MapPage, nameof(MapPage), Characteristics.MAP_PAGE),
                mapper.DropDown(NormalPosition, nameof(NormalPosition), Characteristics.NORMAL_POSITION),
                mapper.Numerical(NormalSysPressure, nameof(NormalSysPressure), Characteristics.NORMAL_SYS_PRESSURE),
                mapper.Numerical(NumberofTurns, nameof(NumberofTurns), Characteristics.NUMBER_OF_TURNS),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OpenDirection, nameof(OpenDirection), Characteristics.OPEN_DIRECTION),
                mapper.DropDown(OperatingNutType, nameof(OperatingNutType), Characteristics.OPERATING_NUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PipeMaterial, nameof(PipeMaterial), Characteristics.PIPE_MATERIAL),
                mapper.DropDown(PressureClass, nameof(PressureClass), Characteristics.PRESSURE_CLASS),
                mapper.String(PressureZone, nameof(PressureZone), Characteristics.PRESSURE_ZONE),
                mapper.String(PressureZoneHGL, nameof(PressureZoneHGL), Characteristics.PRESSURE_ZONE_HGL),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTE),
                mapper.String(SpecialMtnNoteDet, nameof(SpecialMtnNoteDet), Characteristics.SPECIAL_MAINT_NOTE_DETAILS),
                mapper.String(Subdivision, nameof(Subdivision), Characteristics.SUBDIVISION),
                mapper.DropDown(SurfaceCover, nameof(SurfaceCover), Characteristics.SURFACE_COVER),
                mapper.DropDown(SurfaceCoverLocTy, nameof(SurfaceCoverLocTy), Characteristics.SURFACE_COVER_LOC_TP),
                mapper.String(TorqueLimit, nameof(TorqueLimit), Characteristics.TORQUE_LIMIT),
                mapper.DropDown(VLVAccessType, nameof(VLVAccessType), Characteristics.ACCESS_TP),
                mapper.DropDown(VlvOperNutSize, nameof(VlvOperNutSize), Characteristics.VLV_OPER_NUT_SIZE),
                mapper.DropDown(VlvSeatType, nameof(VlvSeatType), Characteristics.VLV_SEAT_TP),
                mapper.DropDown(VlvSpecialVBoxMa, nameof(VlvSpecialVBoxMa), Characteristics.VLV_SPECIAL_V_BOX_MARKING),
                mapper.DropDown(VlvValveSize, nameof(VlvValveSize), Characteristics.VLV_VALVE_SIZE),
                mapper.DropDown(VlvValveType, nameof(VlvValveType), Characteristics.VLV_VALVE_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT)
            };
        }

        #endregion
    }
}