using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ControllerExcelRecord : EquipmentExcelRecordBase<ControllerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 136,
                         EQUIPMENT_PURPOSE = 335;

        public struct Characteristics
        {
            public const int APPLICATION_CNTRLR = 943,
                             CNTRLR_TYP = 1066,
                             COMM1_DEVICE = 1573,
                             COMM1_TP = 941,
                             COMM2_DEVICE = 1147,
                             COMM2_TP = 1570,
                             COMM3_DEVICE = 1424,
                             COMM3_TP = 1440,
                             COMM4_DEVICE = 1463,
                             COMM4_TP = 866,
                             COMM5_DEVICE = 1453,
                             COMM5_TP = 1309,
                             COMM6_DEVICE = 1096,
                             COMM6_TP = 1206,
                             CPU_BATTERY_NUM_OR_NONE = 1878,
                             END_NODE = 1553,
                             NARUC_MAINTENANCE_ACCOUNT = 2038,
                             NARUC_OPERATIONS_ACCOUNT = 2039,
                             OWNED_BY = 1877,
                             REMOTE_IO = 1282,
                             STANDBY_POWER_TP = 981,
                             VOLT_RATING_CNTRLR = 895;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string COMM1Device { get; set; }
        public string COMM1Type { get; set; }
        public string COMM2Device { get; set; }
        public string COMM2Type { get; set; }
        public string COMM3Device { get; set; }
        public string COMM3Type { get; set; }
        public string COMM4Device { get; set; }
        public string COMM4Type { get; set; }
        public string COMM5Device { get; set; }
        public string COMM5Type { get; set; }
        public string COMM6Device { get; set; }
        public string COMM6Type { get; set; }
        public string CPUBatteryorNo { get; set; }
        public string ControllerType { get; set; }
        public string EndNode { get; set; }
        public string OwnedBy { get; set; }
        public string RemoteIO { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CONTROLLER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2304;
        protected override int NARUCSpecialMtnNoteDetailsId => 2305;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CNTRLR),
                mapper.DropDown(COMM1Type, nameof(COMM1Type), Characteristics.COMM1_TP),
                mapper.DropDown(COMM1Device, nameof(COMM1Device), Characteristics.COMM1_DEVICE),
                mapper.DropDown(COMM2Type, nameof(COMM2Type), Characteristics.COMM2_TP),
                mapper.DropDown(COMM2Device, nameof(COMM2Device), Characteristics.COMM2_DEVICE),
                mapper.DropDown(COMM3Type, nameof(COMM3Type), Characteristics.COMM3_TP),
                mapper.DropDown(COMM3Device, nameof(COMM3Device), Characteristics.COMM3_DEVICE),
                mapper.DropDown(COMM4Type, nameof(COMM4Type), Characteristics.COMM4_TP),
                mapper.DropDown(COMM4Device, nameof(COMM4Device), Characteristics.COMM4_DEVICE),
                mapper.DropDown(COMM5Type, nameof(COMM5Type), Characteristics.COMM5_TP),
                mapper.DropDown(COMM5Device, nameof(COMM5Device), Characteristics.COMM5_DEVICE),
                mapper.DropDown(COMM6Type, nameof(COMM6Type), Characteristics.COMM6_TP),
                mapper.DropDown(COMM6Device, nameof(COMM6Device), Characteristics.COMM6_DEVICE),
                mapper.String(CPUBatteryorNo, nameof(CPUBatteryorNo), Characteristics.CPU_BATTERY_NUM_OR_NONE),
                mapper.DropDown(ControllerType, nameof(ControllerType), Characteristics.CNTRLR_TYP),
                mapper.DropDown(EndNode, nameof(EndNode), Characteristics.END_NODE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RemoteIO, nameof(RemoteIO), Characteristics.REMOTE_IO),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING_CNTRLR),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}