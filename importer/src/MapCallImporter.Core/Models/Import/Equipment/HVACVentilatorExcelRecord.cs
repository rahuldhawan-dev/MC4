using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HVACVentilatorExcelRecord : EquipmentExcelRecordBase<HVACVentilatorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 173, EQUIPMENT_PURPOSE = 368;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 807,
                             APPLICATION_HVAC_VNT = 1456,
                             DUTY_CYCLE = 1035,
                             ENERGY_TP = 1327,
                             HVAC_VNT_TYP = 1361,
                             OUTPUT_UOM = 1576,
                             OUTPUT_VALUE = 1918,
                             OWNED_BY = 1917,
                             NARUC_MAINTENANCE_ACCOUNT = 2112,
                             NARUC_OPERATIONS_ACCOUNT = 2113,
                             VOLT_RATING = 1008;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string HVACVentilation { get; set; }
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
        protected override int NARUCSpecialMtnNotesId => 2378;
        protected override int NARUCSpecialMtnNoteDetailsId => 2379;

        protected override string EquipmentType => "HVAC VENTILATOR";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_HVAC_VNT),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(HVACVentilation, nameof(HVACVentilation), Characteristics.HVAC_VNT_TYP),
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