using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class WaterHeaterExcelRecord : EquipmentExcelRecordBase<WaterHeaterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 174,
                         EQUIPMENT_PURPOSE = 428;

        public struct Characteristics
        {
            public const int AMP_RATING = 808,
                             APPLICATION_HVAC_WH = 1468,
                             DUTY_CYCLE = 1355,
                             ENERGY_TP = 1129,
                             HVAC_WH_TYP = 1310,
                             NARUC_MAINTENANCE_ACCOUNT = 2108,
                             NARUC_OPERATIONS_ACCOUNT = 2109,
                             OUTPUT_UOM = 1292,
                             OUTPUT_VALUE = 1920,
                             OWNED_BY = 1919,
                             VOLT_RATING = 1336;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string HVACWaterHeater { get; set; }
        public string MaxPressure { get; set; }
        public string OutputUOM { get; set; }
        public string OutputValue { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TnkVolumegal { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "WATER HEATER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2380;
        protected override int NARUCSpecialMtnNoteDetailsId => 2381;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_HVAC_WH),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(HVACWaterHeater, nameof(HVACWaterHeater), Characteristics.HVAC_WH_TYP),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.String(OutputValue, nameof(OutputValue), Characteristics.OUTPUT_VALUE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}