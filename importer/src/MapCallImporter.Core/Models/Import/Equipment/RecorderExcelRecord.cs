using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class RecorderExcelRecord : EquipmentExcelRecordBase<RecorderExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 208,
                         EQUIPMENT_PURPOSE = 404;

        public struct Characteristics
        {
            public const int INSTRUMENT_RANGE = 1970,
                             INSTRUMENT_UOM = 1010,
                             LOOP_POWER = 1549,
                             NARUC_MAINTENANCE_ACCOUNT = 2185,
                             NARUC_OPERATIONS_ACCOUNT = 2186,
                             NEMA_ENCLOSURE = 1141,
                             ON_SCADA = 1038,
                             OUTPUT_TP = 1049,
                             OWNED_BY = 1969,
                             RECORDER_TYP = 1271;
        }

        #endregion

        #region Properties

        public string DataRecorderType { get; set; }
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

        protected override string EquipmentType => "RECORDER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2446;
        protected override int NARUCSpecialMtnNoteDetailsId => 2533;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(DataRecorderType, nameof(DataRecorderType), Characteristics.RECORDER_TYP),
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