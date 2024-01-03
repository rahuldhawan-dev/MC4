using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class SafetyShowerExcelRecord : EquipmentExcelRecordBase<SafetyShowerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 211,
                         EQUIPMENT_PURPOSE = 407;

        public struct Characteristics
        {
            public const int NARUC_MAINTENANCE_ACCOUNT = 2194,
                             NARUC_OPERATIONS_ACCOUNT = 2195,
                             OWNED_BY = 1971,
                             RETEST_REQUIRED = 1450,
                             SAF_SHWR_TYP = 1348;
        }

        #endregion

        #region Properties

        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SafetyShowerType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "SAFETY SHOWER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2451;
        protected override int NARUCSpecialMtnNoteDetailsId => 2452;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.DropDown(SafetyShowerType, nameof(SafetyShowerType), Characteristics.SAF_SHWR_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}