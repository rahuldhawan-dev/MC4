using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class BurnerExcelRecord : EquipmentExcelRecordBase<BurnerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 127,
                         EQUIPMENT_PURPOSE = 322;

        public struct Characteristics
        {
            public const int AMP_RATING = 814,
                             APPLICATION_BURNER = 1467,
                             BURNER_TYP = 1139,
                             DUTY_CYCLE = 1237,
                             ENERGY_TP = 1248,
                             NARUC_MAINTENANCE_ACCOUNT = 2020,
                             NARUC_OPERATIONS_ACCOUNT = 2021,
                             OUTPUT_UOM = 987,
                             VOLT_RATING = 1088;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string Application { get; set; }
        public string BurnerType { get; set; }
        public string DutyCycle { get; set; }
        public string EnergyType { get; set; }
        public string OutputUOM { get; set; }
        public string OutputValue { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "BURNER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2286;
        protected override int NARUCSpecialMtnNoteDetailsId => 2287;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_BURNER),
                mapper.DropDown(BurnerType, nameof(BurnerType), Characteristics.BURNER_TYP),
                mapper.DropDown(DutyCycle, nameof(DutyCycle), Characteristics.DUTY_CYCLE),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}