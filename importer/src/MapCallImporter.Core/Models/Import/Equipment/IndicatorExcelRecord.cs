using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class IndicatorExcelRecord : EquipmentExcelRecordBase<IndicatorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 176,
                         EQUIPMENT_PURPOSE = 370;

        public struct Characteristics
        {
            public const int INDICATR_TYP = 1281,
                             INSTRUMENT_RANGE = 1943,
                             INSTRUMENT_UOM = 1192,
                             LOOP_POWER = 1277,
                             NARUC_MAINTENANCE_ACCOUNT = 2118,
                             NARUC_OPERATIONS_ACCOUNT = 2119,
                             NEMA_ENCLOSURE = 1376,
                             ON_SCADA = 1430,
                             OUTPUT_TP = 1377,
                             OWNED_BY = 1942;
        }

        #endregion

        #region Properties

        public string IndicatorType { get; set; }
        public string InstrumentRange { get; set; }
        public string InstrumentUOM { get; set; }
        public string LoopPower { get; set; }
        public string NEMAEnclosure { get; set; }
        public string OnSCADA { get; set; }
        public string OutputType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "INDICATOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2384;
        protected override int NARUCSpecialMtnNoteDetailsId => 2385;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(IndicatorType, nameof(IndicatorType), Characteristics.INDICATR_TYP),
                mapper.String(InstrumentRange, nameof(InstrumentRange), Characteristics.INSTRUMENT_RANGE),
                mapper.DropDown(InstrumentUOM, nameof(InstrumentUOM), Characteristics.INSTRUMENT_UOM),
                mapper.DropDown(LoopPower, nameof(LoopPower), Characteristics.LOOP_POWER),
                mapper.DropDown(NEMAEnclosure, nameof(NEMAEnclosure), Characteristics.NEMA_ENCLOSURE),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OutputType, nameof(OutputType), Characteristics.OUTPUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}