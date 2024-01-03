using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HVACHeaterExcelRecord : EquipmentExcelRecordBase<HVACHeaterExcelRecord>
    {
        #region Constants

        public static int EQUIPMENT_TYPE = 171, EQUIPMENT_PURPOSE = 367;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 806,
                             APPLICATION_HVAC_HTR = 972,
                             DUTY_CYCLE = 1550,
                             ENERGY_TP = 922,
                             HVAC_HTR_TYP = 951,
                             OUTPUT_UOM = 996,
                             OUTPUT_VALUE = 1766,
                             OWNED_BY = 1765,
                             SPECIAL_MAINT_NOTES = 1767,
                             NARUC_MAINTENANCE_ACCOUNT = 2114,
                             NARUC_OPERATIONS_ACCOUNT = 2115,
                             VOLT_RATING = 1131;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string HVACHeater { get; set; }
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
        protected override int NARUCSpecialMtnNotesId => 2374;
        protected override int NARUCSpecialMtnNoteDetailsId => 2375;

        protected override string EquipmentType => "HVAC HEATER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_HVAC_HTR),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(HVACHeater, nameof(HVACHeater), Characteristics.HVAC_HTR_TYP),
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