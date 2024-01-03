using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HVACChillerExcelRecord : EquipmentExcelRecordBase<HVACChillerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 167,
                         EQUIPMENT_PURPOSE = 364;

        public struct Characteristics
        {
            public const int AMP_RATING = 803,
                             APPLICATION_HVAC_CHL = 1297,
                             DUTY_CYCLE = 970,
                             ENERGY_TP = 1173,
                             HVAC_CHL_TYP = 916,
                             NARUC_MAINTENANCE_ACCOUNT = 2100,
                             NARUC_OPERATIONS_ACCOUNT = 2101,
                             OUTPUT_UOM = 1278,
                             OUTPUT_VALUE = 1753,
                             OWNED_BY = 1752,
                             SPECIAL_MAINT_NOTES = 1754,
                             VOLT_RATING = 1350;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string HVACChiller { get; set; }
        public string OutputUOM { get; set; }
        public string OutputValue { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "HVAC CHILLER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2366;
        protected override int NARUCSpecialMtnNoteDetailsId => 2367;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_HVAC_CHL),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(HVACChiller, nameof(HVACChiller), Characteristics.HVAC_CHL_TYP),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.String(OutputValue, nameof(OutputValue), Characteristics.OUTPUT_VALUE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}