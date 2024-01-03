using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ChemicalDryFeederExcelRecord : EquipmentExcelRecordBase<ChemicalDryFeederExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 132,
                         EQUIPMENT_PURPOSE = 325;

        public struct Characteristics
        {
            public const int APPLICATION_CHMF_DRY = 899,
                             CHEM_FEED_RATE = 1716,
                             CHM_DOSING_CONTROL = 1552,
                             CHM_FEED_RATE_UOM = 1093,
                             CHM_MATERIAL = 873,
                             CHMF_DRY_TYP = 1232,
                             NARUC_MAINTENANCE_ACCOUNT = 2030,
                             NARUC_OPERATIONS_ACCOUNT = 2031,
                             OWNED_BY = 1715,
                             SPECIAL_MAINT_NOTES = 1717;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string ChmDosingControl { get; set; }
        public string ChmFeedRate { get; set; }
        public string ChmFeedRateUOM { get; set; }
        public string ChmMaterial { get; set; }
        public string DryFeederType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CHEMICAL DRY FEEDER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2296;
        protected override int NARUCSpecialMtnNoteDetailsId => 2297;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CHMF_DRY),
                mapper.DropDown(ChmDosingControl, nameof(ChmDosingControl), Characteristics.CHM_DOSING_CONTROL),
                mapper.String(ChmFeedRate, nameof(ChmFeedRate), Characteristics.CHEM_FEED_RATE),
                mapper.DropDown(ChmFeedRateUOM, nameof(ChmFeedRateUOM), Characteristics.CHM_FEED_RATE_UOM),
                mapper.DropDown(ChmMaterial, nameof(ChmMaterial), Characteristics.CHM_MATERIAL),
                mapper.DropDown(DryFeederType, nameof(DryFeederType), Characteristics.CHMF_DRY_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}