using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class CoolingTowerExcelRecord : EquipmentExcelRecordBase<CoolingTowerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 172,
                         EQUIPMENT_PURPOSE = 337;

        public struct Characteristics
        {
            public const int APPLICATION_HVAC_TWR = 1001,
                             CFM = 1783,
                             ENERGY_TP = 1130,
                             HVAC_TWR_TYP = 1167,
                             NARUC_MAINTENANCE_ACCOUNT = 2110,
                             NARUC_OPERATIONS_ACCOUNT = 2111,
                             OUTPUT_UOM = 1149,
                             OUTPUT_VALUE = 1784,
                             OWNED_BY = 1782,
                             SPECIAL_MAINT_NOTES = 1785;
        }

        #endregion

        #region Properties

        public string HVACCoolingTower { get; set; }
        public string OwnedBy { get; set; }
        public string Application { get; set; }
        public string EnergyType { get; set; }
        public string OutputValue { get; set; }
        public string OutputUOM { get; set; }
        public string CFM { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDetails { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccounut { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "COOLING TOWER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2376;
        protected override int NARUCSpecialMtnNoteDetailsId => 2377;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(HVACCoolingTower, nameof(HVACCoolingTower), Characteristics.HVAC_TWR_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_HVAC_TWR),
                mapper.DropDown(EnergyType, nameof(EnergyType), Characteristics.ENERGY_TP),
                mapper.String(OutputValue, nameof(OutputValue), Characteristics.OUTPUT_VALUE),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.String(CFM, nameof(CFM), Characteristics.CFM),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccounut, nameof(NARUCOperationsAccounut), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}