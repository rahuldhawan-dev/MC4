using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ScaleExcelRecord : EquipmentExcelRecordBase<ScaleExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 213,
                         EQUIPMENT_PURPOSE = 410;

        public struct Characteristics
        {
            public const int INSTRUMENT_RANGE = 1975,
                             INSTRUMENT_UOM = 1106,
                             LOOP_POWER = 1240,
                             NARUC_MAINTENANCE_ACCOUNT = 2199,
                             NARUC_OPERATIONS_ACCOUNT = 2200,
                             NEMA_ENCLOSURE = 1433,
                             ON_SCADA = 995,
                             OUTPUT_TP = 1585,
                             OWNED_BY = 1974,
                             SCALE_TYP = 1530;
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
        public string ScaleType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "SCALE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2455;
        protected override int NARUCSpecialMtnNoteDetailsId => 2456;

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
                mapper.DropDown(ScaleType, nameof(ScaleType), Characteristics.SCALE_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}