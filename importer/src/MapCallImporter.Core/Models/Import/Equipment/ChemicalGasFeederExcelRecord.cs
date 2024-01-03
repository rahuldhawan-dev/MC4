using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ChemicalGasFeederExcelRecord : EquipmentExcelRecordBase<ChemicalGasFeederExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 133,
                         EQUIPMENT_PURPOSE = 326;

        public struct Characteristics
        {
            public const int APPLICATION_CHMF_GAS = 931,
                             CHM_DOSING_CONTROL = 907,
                             CHM_FEED_RATE = 1719,
                             CHM_FEED_RATE_UOM = 1144,
                             CHM_MATERIAL = 1065,
                             CHMF_GAS_TYP = 950,
                             NARUC_MAINTENANCE_ACCOUNT = 2032,
                             NARUC_OPERATIONS_ACCOUNT = 2033,
                             OWNED_BY = 1718,
                             SPECIAL_MAINT_NOTES = 1720;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string ChmDosingControl { get; set; }
        public string ChmFeedRate { get; set; }
        public string ChmFeedRateUOM { get; set; }
        public string ChmMaterial { get; set; }
        public string GasFeederType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CHEMICAL GAS FEEDER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2298;
        protected override int NARUCSpecialMtnNoteDetailsId => 2299;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CHMF_GAS),
                mapper.DropDown(ChmDosingControl, nameof(ChmDosingControl), Characteristics.CHM_DOSING_CONTROL),
                mapper.String(ChmFeedRate, nameof(ChmFeedRate), Characteristics.CHM_FEED_RATE),
                mapper.DropDown(ChmFeedRateUOM, nameof(ChmFeedRateUOM), Characteristics.CHM_FEED_RATE_UOM),
                mapper.DropDown(ChmMaterial, nameof(ChmMaterial), Characteristics.CHM_MATERIAL),
                mapper.DropDown(GasFeederType, nameof(GasFeederType), Characteristics.CHMF_GAS_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}