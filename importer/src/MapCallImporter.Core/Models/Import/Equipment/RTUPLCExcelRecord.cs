using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class RTUPLCExcelRecord : EquipmentExcelRecordBase<RTUPLCExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 209, EQUIPMENT_PURPOSE = 406;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_RTU_PLC = 891,
                             COMM1_DEVICE = 1506,
                             COMM1_TP = 976,
                             COMM2_DEVICE = 1379,
                             COMM2_TP = 1082,
                             COMM3_DEVICE = 944,
                             COMM3_TP = 1558,
                             COMM4_DEVICE = 1076,
                             COMM4_TP = 1521,
                             COMM5_DEVICE = 977,
                             COMM5_TP = 1525,
                             COMM6_DEVICE = 1524,
                             COMM6_TP = 956,
                             CPU_BATTERY = 1647,
                             END_NODE = 1067,
                             NARUC_MAINTENANCE_ACCOUNT = 2187,
                             NARUC_OPERATIONS_ACCOUNT = 2188,
                             NARUC_SPECIAL_MAINT_NOTE_DETAILS = 2448,
                             NARUC_SPECIAL_MAINT_NOTES = 2447,
                             OWNED_BY = 1646,
                             REMOTE_IO = 1242,
                             RTU_PLC_TYP = 988,
                             SPECIAL_MAINT_NOTES_DIST = 1648,
                             STANDBY_POWER_TP = 1118,
                             VOLT_RATING_RTU_PLC = 1489;

            #endregion
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
        public string EndNode { get; set; }
        public string OwnedBy { get; set; }
        public string RTUType { get; set; }
        public string RemoteIO { get; set; }
        public string VoltRating { get; set; }
        public string StandbyPowerType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2447;
        protected override int NARUCSpecialMtnNoteDetailsId => 2448;

        protected override string EquipmentType => "RTU - PLC";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_RTU_PLC),
                mapper.DropDown(COMM1Device, nameof(COMM1Device), Characteristics.COMM1_DEVICE),
                mapper.DropDown(COMM1Type, nameof(COMM1Type), Characteristics.COMM1_TP),
                mapper.DropDown(COMM2Device, nameof(COMM2Device), Characteristics.COMM2_DEVICE),
                mapper.DropDown(COMM2Type, nameof(COMM2Type), Characteristics.COMM2_TP),
                mapper.DropDown(COMM3Device, nameof(COMM3Device), Characteristics.COMM3_DEVICE),
                mapper.DropDown(COMM3Type, nameof(COMM3Type), Characteristics.COMM3_TP),
                mapper.DropDown(COMM4Device, nameof(COMM4Device), Characteristics.COMM4_DEVICE),
                mapper.DropDown(COMM4Type, nameof(COMM4Type), Characteristics.COMM4_TP),
                mapper.DropDown(COMM5Device, nameof(COMM5Device), Characteristics.COMM5_DEVICE),
                mapper.DropDown(COMM5Type, nameof(COMM5Type), Characteristics.COMM5_TP),
                mapper.DropDown(COMM6Device, nameof(COMM6Device), Characteristics.COMM6_DEVICE),
                mapper.DropDown(COMM6Type, nameof(COMM6Type), Characteristics.COMM6_TP),
                mapper.String(CPUBatteryorNo, nameof(CPUBatteryorNo), Characteristics.CPU_BATTERY),
                mapper.DropDown(EndNode, nameof(EndNode), Characteristics.END_NODE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RTUType, nameof(RTUType), Characteristics.RTU_PLC_TYP),
                mapper.DropDown(RemoteIO, nameof(RemoteIO), Characteristics.REMOTE_IO),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING_RTU_PLC),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}