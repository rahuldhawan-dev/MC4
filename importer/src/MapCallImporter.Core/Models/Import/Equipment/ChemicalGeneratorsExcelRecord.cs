using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ChemicalGeneratorsExcelRecord : EquipmentExcelRecordBase<ChemicalGeneratorsExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 130, EQUIPMENT_PURPOSE = 327;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_CHEM_GEN = 1034,
                             CHEM_FEED_RATE = 1874,
                             CHEM_GEN_TYP = 1366,
                             CHM_DOSING_CONTROL = 973,
                             CHM_FEED_RATE_UOM = 1195,
                             CHM_MATERIAL = 1370,
                             NARUC_MAINTENANCE_ACCOUNT = 2026,
                             NARUC_OPERATIONS_ACCOUNT = 2027,
                             OWNED_BY = 1873;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string ChemicalGeneratorT { get; set; }
        public string ChmDosingControl { get; set; }
        public string ChmFeedRate { get; set; }
        public string ChmFeedRateUOM { get; set; }
        public string ChmMaterial { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2292;
        protected override int NARUCSpecialMtnNoteDetailsId => 2293;

        protected override string EquipmentType => "CHEMICAL GENERATORS";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CHEM_GEN),
                mapper.DropDown(ChemicalGeneratorT, nameof(ChemicalGeneratorT), Characteristics.CHEM_GEN_TYP),
                mapper.DropDown(ChmDosingControl, nameof(ChmDosingControl), Characteristics.CHM_DOSING_CONTROL),
                mapper.String(ChmFeedRate, nameof(ChmFeedRate), Characteristics.CHEM_FEED_RATE),
                mapper.DropDown(ChmFeedRateUOM, nameof(ChmFeedRateUOM), Characteristics.CHM_FEED_RATE_UOM),
                mapper.DropDown(ChmMaterial, nameof(ChmMaterial), Characteristics.CHM_MATERIAL),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}