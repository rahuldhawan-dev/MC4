using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class DistributionToolExcelRecord : EquipmentExcelRecordBase<DistributionToolExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 151,
                         EQUIPMENT_PURPOSE = 341;

        public struct Characteristics
        {
            public const int DISTTOOL_DETECTOR_TYPE = 1177,
                             DISTTOOL_MAX_SENSORS = 849,
                             DISTTOOL_SENSOR_TYPE = 1545,
                             DISTTOOL_TYP = 1184,
                             NARUC_MAINTENANCE_ACCOUNT = 2068,
                             NARUC_OPERATIONS_ACCOUNT = 2069;
        }

        #endregion

        #region Properties

        public string DetectorType { get; set; }
        public string DistributionToolTy { get; set; }
        public string MaxofSensors { get; set; }
        public string OwnedBy { get; set; }
        public string SensorType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "DISTRIBUTION TOOL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2336;
        protected override int NARUCSpecialMtnNoteDetailsId => 2337;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(DetectorType, nameof(DetectorType), Characteristics.DISTTOOL_DETECTOR_TYPE),
                mapper.DropDown(DistributionToolTy, nameof(DistributionToolTy), Characteristics.DISTTOOL_TYP),
                mapper.Numerical(MaxofSensors, nameof(MaxofSensors), Characteristics.DISTTOOL_MAX_SENSORS),
                mapper.DropDown(SensorType, nameof(SensorType), Characteristics.DISTTOOL_SENSOR_TYPE),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}