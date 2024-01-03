using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FlowWeirExcelRecord : EquipmentExcelRecordBase<FlowWeirExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 161,
                         EQUIPMENT_PURPOSE = 356;

        public struct Characteristics
        {
            public const int APPLICATION_FLO_WEIR = 1330,
                             BYPASS_VALVE = 1363,
                             CALIBRATION_UOM = 1057,
                             COMM_PROTOCOL = 1480,
                             FLOW_UOM = 1157,
                             FLO_WEIR_TYP = 1133,
                             INSTRUMENT_POWER = 1385,
                             MET_MATERIAL = 1014,
                             NARUC_MAINTENANCE_ACCOUNT = 2088,
                             NARUC_OPERATIONS_ACCOUNT = 2089,
                             NOMINAL_SIZE = 1306,
                             ON_SCADA = 1163,
                             OUTPUT_TP = 912,
                             SQUARE_ROOT = 1423,
                             TRANSMITTER = 969,
                             TRANSMITTER_MANUFACTURER = 1526;
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
        public string FlowWeirType { get; set; }
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

        protected override string EquipmentType => "FLOW WEIR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2354;
        protected override int NARUCSpecialMtnNoteDetailsId => 2355;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_FLO_WEIR),
                mapper.DropDown(BypassValve, nameof(BypassValve), Characteristics.BYPASS_VALVE),
                mapper.DropDown(CalibrationUOM, nameof(CalibrationUOM), Characteristics.CALIBRATION_UOM),
                mapper.DropDown(CommProtocol, nameof(CommProtocol), Characteristics.COMM_PROTOCOL),
                mapper.DropDown(FlowUOM, nameof(FlowUOM), Characteristics.FLOW_UOM),
                mapper.DropDown(FlowWeirType, nameof(FlowWeirType), Characteristics.FLO_WEIR_TYP),
                mapper.DropDown(InstrumentPower, nameof(InstrumentPower), Characteristics.INSTRUMENT_POWER),
                mapper.DropDown(MetMaterial, nameof(MetMaterial), Characteristics.MET_MATERIAL),
                mapper.DropDown(NominalSize, nameof(NominalSize), Characteristics.NOMINAL_SIZE),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OutputType, nameof(OutputType), Characteristics.OUTPUT_TP),
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