using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class HeatExchangerExcelRecord : EquipmentExcelRecordBase<HeatExchangerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 170,
                         EQUIPMENT_PURPOSE = 362;

        public struct Characteristics
        {
            public const int HVAC_EXC_TYP = 1101,
                             MATERIAL_2 = 1054,
                             MATERIAL_OF_CONSTRUCTION_HVAC = 902,
                             MAX_PRESSURE = 1763,
                             MEDIA_1_TP_HVAC_EXC = 1337,
                             MEDIA_2_TP_HVAC_EXC = 952,
                             NARUC_MAINTENANCE_ACCOUNT = 2106,
                             NARUC_OPERATIONS_ACCOUNT = 2107,
                             OUTPUT_UOM = 1571,
                             OUTPUT_VALUE = 1762,
                             OWNED_BY = 1761,
                             SPECIAL_MAINT_NOTES = 1764;
        }

        #endregion

        #region Properties

        public string HVACExchanger { get; set; }
        public string Material2 { get; set; }
        public string MaterialofConstruc { get; set; }
        public string MaxPressure { get; set; }
        public string Media1Type { get; set; }
        public string Media2Type { get; set; }
        public string OutputUOM { get; set; }
        public string OutputValue { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "HEAT EXCHANGER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2372;
        protected override int NARUCSpecialMtnNoteDetailsId => 2373;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(HVACExchanger, nameof(HVACExchanger), Characteristics.HVAC_EXC_TYP),
                mapper.DropDown(Material2, nameof(Material2), Characteristics.MATERIAL_2),
                mapper.DropDown(MaterialofConstruc, nameof(MaterialofConstruc), Characteristics.MATERIAL_OF_CONSTRUCTION_HVAC),
                mapper.String(MaxPressure, nameof(MaxPressure), Characteristics.MAX_PRESSURE),
                mapper.DropDown(Media1Type, nameof(Media1Type), Characteristics.MEDIA_1_TP_HVAC_EXC),
                mapper.DropDown(Media2Type, nameof(Media2Type), Characteristics.MEDIA_2_TP_HVAC_EXC),
                mapper.DropDown(OutputUOM, nameof(OutputUOM), Characteristics.OUTPUT_UOM),
                mapper.String(OutputValue, nameof(OutputValue), Characteristics.OUTPUT_VALUE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}