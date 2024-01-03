using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class BoilerExcelRecord : EquipmentExcelRecordBase<BoilerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 126,
                         EQUIPMENT_PURPOSE = 321;

        public struct Characteristics
        {
            public const int AMP_RATING = 813,
                             APPLICATION_BOILER = 1505,
                             BOILER_TYP = 1191,
                             DUTY_CYCLE = 1288,
                             ENERGY_TP = 1084,
                             NARUC_MAINTENANCE_ACCOUNT = 2018,
                             NARUC_OPERATIONS_ACCOUNT = 2019,
                             OUTPUT_UOM = 1556,
                             OUTPUT_VALUE = 1711,
                             OWNED_BY = 1710,
                             SPECIAL_MAINT_NOTES = 1712,
                             VOLT_RATING = 1444;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string BoilerType { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string OutputUOM { get; set; }
        public string OutputValue { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "BOILER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2284;
        protected override int NARUCSpecialMtnNoteDetailsId => 2285;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_BOILER),
                mapper.DropDown(BoilerType, nameof(BoilerType), Characteristics.BOILER_TYP),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.String(OutputValue, nameof(OutputValue), Characteristics.OUTPUT_VALUE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}