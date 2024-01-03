using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class InstrumentSwitchExcelRecord : EquipmentExcelRecordBase<InstrumentSwitchExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 177, EQUIPMENT_PURPOSE = 371;

        public struct Characteristics
        {
            #region Constants

            public const int INSTRUMENT_RANGE = 1945,
                             INSTRUMENT_UOM = 1561,
                             INST_SW_TYP = 1204,
                             LOOP_POWER = 1539,
                             NEMA_ENCLOSURE = 1226,
                             ON_SCADA = 1331,
                             OUTPUT_TP = 978,
                             OWNED_BY = 1944,
                             NARUC_MAINTENANCE_ACCOUNT = 2120,
                             NARUC_OPERATIONS_ACCOUNT = 2121;

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
        public string SwitchType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2386;
        protected override int NARUCSpecialMtnNoteDetailsId => 2387;

        protected override string EquipmentType => "INSTRUMENT SWITCH";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.String(InstrumentRange, nameof(InstrumentRange), Characteristics.INSTRUMENT_RANGE),
                mapper.DropDown(InstrumentUOM, nameof(InstrumentUOM), Characteristics.INSTRUMENT_UOM),
                mapper.DropDown(LoopPower, nameof(LoopPower), Characteristics.LOOP_POWER),
                mapper.DropDown(NEMAEnclosure, nameof(NEMAEnclosure), Characteristics.NEMA_ENCLOSURE),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.DropDown(OutputType, nameof(OutputType), Characteristics.OUTPUT_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(SwitchType, nameof(SwitchType), Characteristics.INST_SW_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}