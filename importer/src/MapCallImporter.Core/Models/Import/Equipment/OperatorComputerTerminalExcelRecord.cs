using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class OperatorComputerTerminalExcelRecord : EquipmentExcelRecordBase<OperatorComputerTerminalExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 186,
                         EQUIPMENT_PURPOSE = 384;

        public struct Characteristics
        {
            public const int APPLICATION_OIT = 1412,
                             HMI_MANUFACTURER = 881,
                             NARUC_MAINTENANCE_ACCOUNT = 2136,
                             NARUC_OPERATIONS_ACCOUNT = 2137,
                             NETWORK_SCHEME = 1410,
                             OIT_TYP = 1589,
                             OPERATING_SYSTEMS = 1112,
                             RAID = 1222,
                             STANDBY_POWER_TP = 1152;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string HMISoftware { get; set; }
        public string NetworkScheme { get; set; }
        public string OITType { get; set; }
        public string OperatingSystem { get; set; }
        public string OwnedBy { get; set; }
        public string RAID { get; set; }
        public string RAMMemory { get; set; }
        public string SoftwareLicense { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "OPERATOR COMPUTER TERMINAL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2402;
        protected override int NARUCSpecialMtnNoteDetailsId => 2403;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_OIT),
                mapper.DropDown(HMISoftware, nameof(HMISoftware), Characteristics.HMI_MANUFACTURER),
                mapper.DropDown(NetworkScheme, nameof(NetworkScheme), Characteristics.NETWORK_SCHEME),
                mapper.DropDown(OITType, nameof(OITType), Characteristics.OIT_TYP),
                mapper.DropDown(OperatingSystem, nameof(OperatingSystem), Characteristics.OPERATING_SYSTEMS),
                mapper.DropDown(RAID, nameof(RAID), Characteristics.RAID),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}