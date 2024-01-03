using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ChemicalPipingExcelRecord : EquipmentExcelRecordBase<ChemicalPipingExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 131, EQUIPMENT_PURPOSE = 329;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_CHEM_PIP = 1465,
                             CHEM_PIP_TYP = 1197,
                             CHM_DOSING_CONTROL = 915,
                             CHEM_FEED_RATE = 1876,
                             CHM_FEED_RATE_UOM = 974,
                             CHM_MATERIAL = 1473,
                             NARUC_MAINTENANCE_ACCOUNT = 2028,
                             NARUC_OPERATIONS_ACCOUNT = 2029,
                             OWNED_BY = 1875;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string ChemicalPipingType { get; set; }
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
        protected override int NARUCSpecialMtnNotesId => 2294;
        protected override int NARUCSpecialMtnNoteDetailsId => 2295;

        protected override string EquipmentType => "CHEMICAL PIPING";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CHEM_PIP),
                mapper.DropDown(ChemicalPipingType, nameof(ChemicalPipingType), Characteristics.CHEM_PIP_TYP),
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