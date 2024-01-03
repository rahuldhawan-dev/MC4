using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PDMToolExcelRecord : EquipmentExcelRecordBase<PDMToolExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 188,
                         EQUIPMENT_PURPOSE = 386;

        public struct Characteristics
        {
            public const int NARUC_MAINTENANCE_ACCOUNT = 2141,
                             NARUC_OPERATIONS_ACCOUNT = 2142,
                             PDMTOOL_TYP = 1134,
                             RETEST_REQUIRED = 958;
        }

        #endregion

        #region Properties

        public string OwnedBy { get; set; }
        public string PDMToolType { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "PDM TOOL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2406;
        protected override int NARUCSpecialMtnNoteDetailsId => 2407;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(PDMToolType, nameof(PDMToolType), Characteristics.PDMTOOL_TYP),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}