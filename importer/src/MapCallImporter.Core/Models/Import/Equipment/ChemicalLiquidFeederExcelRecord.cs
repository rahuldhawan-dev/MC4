using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ChemicalLiquidFeederExcelRecord : EquipmentExcelRecordBase<ChemicalLiquidFeederExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 134;
        public const int EQUIPMENT_PURPOSE = 328;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_CHMF_LIQ = 1319,
                             CHEM_FEED_RATE = 1722,
                             CHM_DOSING_CONTROL = 1253,
                             CHM_FEED_RATE_UOM = 1132,
                             CHM_MATERIAL = 1213,
                             CHMF_LIQ_TYP = 1094,
                             NARUC_MAINTENANCE_ACCOUNT = 2034,
                             NARUC_OPERATIONS_ACCOUNT = 2035,
                             OWNED_BY = 1721,
                             SPECIAL_MIANT_NOTES = 1983;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string ChmDosingControl { get; set; }
        public string ChmFeedRate { get; set; }
        public string ChmFeedRateUOM { get; set; }
        public string ChmMaterial { get; set; }
        public string LiquidFeederType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2300;
        protected override int NARUCSpecialMtnNoteDetailsId => 2301;

        protected override string EquipmentType => "CHEMICAL LIQUID FEEDER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CHMF_LIQ),
                mapper.DropDown(ChmDosingControl, nameof(ChmDosingControl), Characteristics.CHM_DOSING_CONTROL),
                mapper.String(ChmFeedRate, nameof(ChmFeedRate), Characteristics.CHEM_FEED_RATE),
                mapper.DropDown(ChmFeedRateUOM, nameof(ChmFeedRateUOM), Characteristics.CHM_FEED_RATE_UOM),
                mapper.DropDown(ChmMaterial, nameof(ChmMaterial), Characteristics.CHM_MATERIAL),
                mapper.DropDown(LiquidFeederType, nameof(LiquidFeederType), Characteristics.CHMF_LIQ_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MIANT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}