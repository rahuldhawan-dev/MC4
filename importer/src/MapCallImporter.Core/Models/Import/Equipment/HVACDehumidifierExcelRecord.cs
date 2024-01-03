using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HVACDehumidifierExcelRecord : EquipmentExcelRecordBase<HVACDehumidifierExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 169, EQUIPMENT_PURPOSE = 366;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 805,
                             APPLICATION_HVAC_DHM = 1531,
                             DUTY_CYCLE = 1538,
                             ENERGY_TP = 1560,
                             HVAC_DHM_TYP = 1086,
                             OUTPUT_UOM = 1154,
                             OUTPUT_VALUE = 1759,
                             OWNED_BY = 1758,
                             SPECIAL_MAINT_NOTES = 1760,
                             VOLT_RATING = 1030,
                             NARUC_MAINTENANCE_ACCOUNT = 2104,
                             NARUC_OPERATIONS_ACCOUNT = 2105;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string HVACHumidifier { get; set; }
        public string OutputUOM { get; set; }
        public string OutputValue { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2370;
        protected override int NARUCSpecialMtnNoteDetailsId => 2371;

        protected override string EquipmentType => "HVAC DEHUMIDIFIER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_HVAC_DHM),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(HVACHumidifier, nameof(HVACHumidifier), Characteristics.HVAC_DHM_TYP),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.String(OutputValue, nameof(OutputValue), Characteristics.OUTPUT_VALUE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
