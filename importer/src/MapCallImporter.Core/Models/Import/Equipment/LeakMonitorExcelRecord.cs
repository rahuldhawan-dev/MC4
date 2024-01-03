using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class LeakMonitorExcelRecord : EquipmentExcelRecordBase<LeakMonitorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 180,
                         EQUIPMENT_PURPOSE = 374;

        public struct Characteristics
        {
            public const int LK_MON_DATA_RETRIEVE_METHOD = 1566,
                             LK_MON_DEPLOYMENT_TYPE = 1304,
                             LK_MON_LOC_TYPE = 992,
                             LK_MON_TYP = 1113,
                             LK_MON_TYPE = 1498,
                             NARUC_MAINTENANCE_ACCOUNT = 2126,
                             NARUC_OPERATIONS_ACCOUNT = 2127;
        }

        #endregion

        #region Properties

        public string LeakMonitorType { get; set; }
        public string OwnedBy { get; set; }
        public string LeakMonitorCorrelationType { get; set; }
        public string LocationType { get; set; }
        public string DataRetrievalMethod { get; set; }
        public string DeploymentType { get; set; }
        public string LocationReference { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "LEAK MONITOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2392;
        protected override int NARUCSpecialMtnNoteDetailsId => 2393;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(LeakMonitorType, nameof(LeakMonitorType), Characteristics.LK_MON_TYP),
                mapper.DropDown(LeakMonitorCorrelationType, nameof(LeakMonitorCorrelationType), Characteristics.LK_MON_TYPE),
                mapper.DropDown(LocationType, nameof(LocationType), Characteristics.LK_MON_LOC_TYPE),
                mapper.DropDown(DataRetrievalMethod, nameof(DataRetrievalMethod), Characteristics.LK_MON_DATA_RETRIEVE_METHOD),
                mapper.DropDown(DeploymentType, nameof(DeploymentType), Characteristics.LK_MON_DEPLOYMENT_TYPE),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}