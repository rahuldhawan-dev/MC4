using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FlowMeterExcelRecord : EquipmentExcelRecordBase<FlowMeterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 160,
                         EQUIPMENT_PURPOSE = 355;

        public struct Characteristics
        {
            public const int APPLICATION_FLO_MET = 1436,
                             BETA_RATIO = 1914,
                             BYPASS_VALVE = 1342,
                             CALIBRATION_UOM = 1176,
                             COMM_PROTOCOL = 1320,
                             DIFFERENTIAL_PSI_RANGE = 1913,
                             FLO_MET_TYP = 1548,
                             FLOW_CALIBRATED_RANGE = 1908,
                             FLOW_MAX_INTERMITTENT = 1910,
                             FLOW_NORMAL_RANGE = 1909,
                             FLOW_UOM = 1025,
                             FLOW_VELOCITY_RANGE = 1911,
                             INSTRUMENT_POWER = 1311,
                             MAX_PRESSURE = 1912,
                             MET_MATERIAL = 1369,
                             NARUC_MAINTENANCE_ACCOUNT = 2086,
                             NARUC_OPERATIONS_ACCOUNT = 2087,
                             NARUC_SPECIAL_MAINT_NOTE_DETAILS = 2353,
                             NARUC_SPECIAL_MAINT_NOTES = 2352,
                             NOMINAL_SIZE = 869,
                             ON_SCADA = 1419,
                             OUTPUT_TP = 1126,
                             OWNED_BY = 1907,
                             PRESSURE_DROP_MAX = 1916,
                             SPECIFIC_GRAVITY = 1915,
                             SQUARE_ROOT = 1302,
                             TRANSMITTER = 1262,
                             TRANSMITTER_MANUFACTURER = 1098;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BetaRatio { get; set; }
        public string BypassValve { get; set; }
        public string CalibrationUOM { get; set; }
        public string CommProtocol { get; set; }
        public string DifferentialPSIRan { get; set; }
        public string FlowCalibratedRang { get; set; }
        public string FlowMaxIntermitten { get; set; }
        public string FlowNormalRange { get; set; }
        public string FlowUOM { get; set; }
        public string FlowVelocityRange { get; set; }
        public string FlowmeterType { get; set; }
        public string InstrumentPower { get; set; }
        public string MaxPressure { get; set; }
        public string MetMaterial { get; set; }
        public string NominalSize { get; set; }
        public string OnSCADA { get; set; }
        public string OutputType { get; set; }
        public string OwnedBy { get; set; }
        public string PressureDropMax { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SpecificGravity { get; set; }
        public string SquareRoot { get; set; }
        public string TransmitterManufact { get; set; }
        public string Transmitter { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2352;
        protected override int NARUCSpecialMtnNoteDetailsId => 2353;

        protected override string EquipmentType => "FLOW METER (NON PREMISE)";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_FLO_MET),
                mapper.String(BetaRatio, nameof(BetaRatio), Characteristics.BETA_RATIO),
                mapper.DropDown(BypassValve, nameof(BypassValve), Characteristics.BYPASS_VALVE),
                mapper.DropDown(CalibrationUOM, nameof(CalibrationUOM), Characteristics.CALIBRATION_UOM),
                mapper.DropDown(CommProtocol, nameof(CommProtocol), Characteristics.COMM_PROTOCOL),
                mapper.String(DifferentialPSIRan, nameof(DifferentialPSIRan), Characteristics.DIFFERENTIAL_PSI_RANGE),
                mapper.String(FlowCalibratedRang, nameof(FlowCalibratedRang), Characteristics.FLOW_CALIBRATED_RANGE),
                mapper.String(FlowMaxIntermitten, nameof(FlowMaxIntermitten), Characteristics.FLOW_MAX_INTERMITTENT),
                mapper.String(FlowNormalRange, nameof(FlowNormalRange), Characteristics.FLOW_NORMAL_RANGE),
                mapper.DropDown(FlowUOM, nameof(FlowUOM), Characteristics.FLOW_UOM),
                mapper.String(FlowVelocityRange, nameof(FlowVelocityRange), Characteristics.FLOW_VELOCITY_RANGE),
                mapper.DropDown(FlowmeterType, nameof(FlowmeterType), Characteristics.FLO_MET_TYP),
                mapper.DropDown(InstrumentPower, nameof(InstrumentPower), Characteristics.INSTRUMENT_POWER),
                mapper.String(MaxPressure, nameof(MaxPressure), Characteristics.MAX_PRESSURE),
                mapper.DropDown(MetMaterial, nameof(MetMaterial), Characteristics.MET_MATERIAL),
                mapper.DropDown(NominalSize, nameof(NominalSize), Characteristics.NOMINAL_SIZE),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OutputType, nameof(OutputType), Characteristics.OUTPUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(PressureDropMax, nameof(PressureDropMax), Characteristics.PRESSURE_DROP_MAX),
                mapper.String(SpecificGravity, nameof(SpecificGravity), Characteristics.SPECIFIC_GRAVITY),
                mapper.DropDown(SquareRoot, nameof(SquareRoot), Characteristics.SQUARE_ROOT),
                mapper.DropDown(TransmitterManufact, nameof(TransmitterManufact), Characteristics.TRANSMITTER_MANUFACTURER),
                mapper.DropDown(Transmitter, nameof(Transmitter), Characteristics.TRANSMITTER),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}