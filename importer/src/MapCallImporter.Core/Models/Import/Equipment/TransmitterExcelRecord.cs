using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class TransmitterExcelRecord : EquipmentExcelRecordBase<TransmitterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 240, EQUIPMENT_PURPOSE = 421;

        public struct Characteristics
        {
            #region Constants

            public const int INSTRUMENT_RANGE = 1750,
                             INSTRUMENT_UOM = 1386,
                             LOOP_POWER = 1543,
                             NEMA_ENCLOSURE = 1494,
                             NARUC_MAINTENANCE_ACCOUNT = 2263,
                             NARUC_OPERATIONS_ACCOUNT = 2264,
                             ON_SCADA = 918,
                             OUTPUT_TP = 1414,
                             OWNED_BY = 1749,
                             XMTR_TYP = 1353;

            #endregion
        }

        #endregion

        #region Properties

        public string InstrumentRange { get; set; }
        public string InstrumentUOM { get; set; }
        public string LoopPower { get; set; }
        public string NEMAEnclosure { get; set; }
        public string OnSCADA { get; set; }
        public string OutputType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TransmitterType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2509;
        protected override int NARUCSpecialMtnNoteDetailsId => 2510;

        protected override string EquipmentType => "TRANSMITTER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(InstrumentRange, nameof(InstrumentRange), Characteristics.INSTRUMENT_RANGE),
                mapper.DropDown(InstrumentUOM, nameof(InstrumentUOM), Characteristics.INSTRUMENT_UOM),
                mapper.DropDown(LoopPower, nameof(LoopPower), Characteristics.LOOP_POWER),
                mapper.DropDown(NEMAEnclosure, nameof(NEMAEnclosure), Characteristics.NEMA_ENCLOSURE),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OutputType, nameof(OutputType), Characteristics.OUTPUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(TransmitterType, nameof(TransmitterType), Characteristics.XMTR_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
