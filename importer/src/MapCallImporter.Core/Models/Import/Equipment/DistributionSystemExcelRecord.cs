using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class DistributionSystemExcelRecord : EquipmentExcelRecordBase<DistributionSystemExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 150,
                         EQUIPMENT_PURPOSE = 340;

        public struct Characteristics
        {
            public const int DEPENDENCY_DRIVER_1 = 837,
                             DISTSYS_TYP = 948,
                             NARUC_MAINTENANCE_ACCOUNT = 2066,
                             NARUC_OPERATIONS_ACCOUNT = 2067;
        }

        #endregion

        #region Properties

        public string DistributionSystemType { get; set; }
        public string OwnedBy { get; set; }
        public string DependencyDriver1 { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "DISTRIBUTION SYSTEM";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2332;
        protected override int NARUCSpecialMtnNoteDetailsId => 2333;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(DistributionSystemType, nameof(DistributionSystemType), Characteristics.DISTSYS_TYP),
                mapper.String(DependencyDriver1, nameof(DependencyDriver1), Characteristics.DEPENDENCY_DRIVER_1),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}