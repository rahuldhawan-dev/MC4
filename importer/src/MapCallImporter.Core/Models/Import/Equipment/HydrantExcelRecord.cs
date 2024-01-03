using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HydrantExcelRecord : EquipmentExcelRecordBase<HydrantExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 175,
                         EQUIPMENT_PURPOSE = 369;

        public struct Characteristics
        {
            public const int BOOK_PAGE = 1927,
                             DEPENDENCY_DRIVER_1 = 840,
                             DEPENDENCY_DRIVER_2 = 846,
                             EAM_PIPE_SIZE = 1046,
                             ECIS_ACCOUNT = 1939,
                             GEOACCURACY_GIS_DATASOURCETYPE = 1941,
                             HISTORICAL_ID = 1940,
                             HYD_AUX_VALVE_BRANCH_SIZE = 1289,
                             HYD_AUXILLARY_VALVE = 1493,
                             HYD_BARREL_SIZE = 990,
                             HYD_BARREL_TP = 1280,
                             HYD_BILLING_TP = 1484,
                             HYD_BURY_DEPTH = 1928,
                             HYD_COLOR_CODE_METHOD = 984,
                             HYD_COLOR_CODE_TP = 1021,
                             HYD_COLORCODE = 1418,
                             HYD_DEAD_END_MAIN = 1518,
                             HYD_LOCK_DEVICE_TP = 991,
                             HYD_OUTLET_CONFIG = 1207,
                             HYD_SIDE_NOZZLE_SIZE = 1019,
                             HYD_STEAMER_SIZE = 1068,
                             HYD_STEAMER_THREAD_TP = 955,
                             HYD_STEM_LUBE = 1174,
                             HYD_TYP = 936,
                             HYD_AUX_VALVE = 1931,
                             HYD_BRANCH_LENGTH = 1930,
                             HYD_EXTENSIONS_AND_SIZES = 1929,
                             HYD_FIRE_DISTRICT = 1938,
                             HYD_SIDE_PORT_THREAD_TYPE = 1933,
                             INSTALLATION_WO = 1936,
                             JOINT_TP = 1107,
                             MAP_PAGE = 1926,
                             NARUC_MAINTENANCE_ACCOUNT = 2116,
                             NARUC_OPERATIONS_ACCOUNT = 2117,
                             NORMAL_SYS_PRESSURE = 857,
                             OPEN_DIRECTION = 1011,
                             OWNED_BY = 1921,
                             PIPE_CHANNEL_SIZE = 1935,
                             PIPE_MATERIAL = 1178,
                             PRESSURE_ZONE = 1924,
                             PRESSURE_CLASS = 1502,
                             PRESSURE_ZONE_HGL = 1925,
                             REPAIR_KIT = 1934,
                             SKETCH = 1937,
                             SPECIAL_MAINT_NOTE = 1922,
                             SPECIAL_MAINT_NOTES_DIST = 1428,
                             SUBDIVISION = 1923;
        }

        #endregion

        #region Properties

        public string BookPage { get; set; }
        public string DependencyDriver1 { get; set; }
        public string DependencyDriver2 { get; set; }
        public string ECISAccount { get; set; }
        public string GEOACCURACYGISDATA { get; set; }
        public string HistoricalID { get; set; }
        public string HydAuxValve { get; set; }
        public string HydAuxValveBranch { get; set; }
        public string HydAuxillaryValve { get; set; }
        public string HydBarrelSize { get; set; }
        public string HydBarrelType { get; set; }
        public string HydBillingType { get; set; }
        public string HydBranchLength { get; set; }
        public string HydBuryDepth { get; set; }
        public string HydColorColorCod { get; set; }
        public string HydColorCodeMetho { get; set; }
        public string HydColorCodeType { get; set; }
        public string HydDeadEndMain { get; set; }
        public string HydExtensionsSiz { get; set; }
        public string HydFireDistrict { get; set; }
        public string HydLockDeviceType { get; set; }
        public string HydOutletConfig { get; set; }
        public string HydSideNozzleSize { get; set; }
        public string HydSidePortThread { get; set; }
        public string HydSteamerSize { get; set; }
        public string HydSteamerThreadT { get; set; }
        public string HydStemLube { get; set; }
        public string HydrantType { get; set; }
        public string InstallationWO { get; set; }
        public string JointType { get; set; }
        public string MapPage { get; set; }
        public string NormalSysPressure { get; set; }
        public string OpenDirection { get; set; }
        public string OwnedBy { get; set; }
        public string PipeChannelSize { get; set; }
        public string PipeMaterial { get; set; }
        public string PressureClass { get; set; }
        public string PressureZone { get; set; }
        public string PressureZoneHGL { get; set; }
        public string RepairKit { get; set; }
        public string Sketch { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string Subdivision { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "HYDRANT";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2382;
        protected override int NARUCSpecialMtnNoteDetailsId => 2383;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(BookPage, nameof(BookPage), Characteristics.BOOK_PAGE),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.String(DependencyDriver2, nameof(DependencyDriver2), Characteristics.DEPENDENCY_DRIVER_2),
                mapper.String(ECISAccount, nameof(ECISAccount), Characteristics.ECIS_ACCOUNT),
                mapper.String(GEOACCURACYGISDATA, nameof(GEOACCURACYGISDATA), Characteristics.GEOACCURACY_GIS_DATASOURCETYPE),
                mapper.String(HistoricalID, nameof(HistoricalID), Characteristics.HISTORICAL_ID),
                mapper.String(HydAuxValve, nameof(HydAuxValve), Characteristics.HYD_AUX_VALVE),
                mapper.DropDown(HydAuxValveBranch, nameof(HydAuxValveBranch), Characteristics.HYD_AUX_VALVE_BRANCH_SIZE),
                mapper.DropDown(HydAuxillaryValve, nameof(HydAuxillaryValve), Characteristics.HYD_AUXILLARY_VALVE),
                mapper.DropDown(HydBarrelSize, nameof(HydBarrelSize), Characteristics.HYD_BARREL_SIZE),
                mapper.DropDown(HydBarrelType, nameof(HydBarrelType), Characteristics.HYD_BARREL_TP),
                mapper.DropDown(HydBillingType, nameof(HydBillingType), Characteristics.HYD_BILLING_TP),
                mapper.String(HydBranchLength, nameof(HydBranchLength), Characteristics.HYD_BRANCH_LENGTH),
                mapper.String(HydBuryDepth, nameof(HydBuryDepth), Characteristics.HYD_BURY_DEPTH),
                mapper.DropDown(HydColorColorCod, nameof(HydColorColorCod), Characteristics.HYD_COLORCODE),
                mapper.DropDown(HydColorCodeMetho, nameof(HydColorCodeMetho), Characteristics.HYD_COLOR_CODE_METHOD),
                mapper.DropDown(HydColorCodeType, nameof(HydColorCodeType), Characteristics.HYD_COLOR_CODE_TP),
                mapper.DropDown(HydDeadEndMain, nameof(HydDeadEndMain), Characteristics.HYD_DEAD_END_MAIN),
                mapper.String(HydExtensionsSiz, nameof(HydExtensionsSiz), Characteristics.HYD_EXTENSIONS_AND_SIZES),
                mapper.String(HydFireDistrict, nameof(HydFireDistrict), Characteristics.HYD_FIRE_DISTRICT),
                mapper.DropDown(HydLockDeviceType, nameof(HydLockDeviceType), Characteristics.HYD_LOCK_DEVICE_TP),
                mapper.DropDown(HydOutletConfig, nameof(HydOutletConfig), Characteristics.HYD_OUTLET_CONFIG),
                mapper.DropDown(HydSideNozzleSize, nameof(HydSideNozzleSize), Characteristics.HYD_SIDE_NOZZLE_SIZE),
                mapper.String(HydSidePortThread, nameof(HydSidePortThread), Characteristics.HYD_SIDE_PORT_THREAD_TYPE),
                mapper.DropDown(HydSteamerSize, nameof(HydSteamerSize), Characteristics.HYD_STEAMER_SIZE),
                mapper.DropDown(HydSteamerThreadT, nameof(HydSteamerThreadT), Characteristics.HYD_STEAMER_THREAD_TP),
                mapper.DropDown(HydStemLube, nameof(HydStemLube), Characteristics.HYD_STEM_LUBE),
                mapper.DropDown(HydrantType, nameof(HydrantType), Characteristics.HYD_TYP),
                mapper.String(InstallationWO, nameof(InstallationWO), Characteristics.INSTALLATION_WO),
                mapper.DropDown(JointType, nameof(JointType), Characteristics.JOINT_TP),
                mapper.String(MapPage, nameof(MapPage), Characteristics.MAP_PAGE),
                mapper.Numerical(NormalSysPressure, nameof(NormalSysPressure), Characteristics.NORMAL_SYS_PRESSURE),
                mapper.DropDown(OpenDirection, nameof(OpenDirection), Characteristics.OPEN_DIRECTION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(PipeChannelSize, nameof(PipeChannelSize), Characteristics.PIPE_CHANNEL_SIZE),
                mapper.DropDown(PipeMaterial, nameof(PipeMaterial), Characteristics.PIPE_MATERIAL),
                mapper.DropDown(PressureClass, nameof(PressureClass), Characteristics.PRESSURE_CLASS),
                mapper.String(PressureZone, nameof(PressureZone), Characteristics.PRESSURE_ZONE),
                mapper.String(PressureZoneHGL, nameof(PressureZoneHGL), Characteristics.PRESSURE_ZONE_HGL),
                mapper.String(RepairKit, nameof(RepairKit), Characteristics.REPAIR_KIT),
                mapper.String(Sketch, nameof(Sketch), Characteristics.SKETCH),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTE),
                mapper.String(Subdivision, nameof(Subdivision), Characteristics.SUBDIVISION),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}