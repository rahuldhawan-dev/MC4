using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PCExcelRecord : EquipmentExcelRecordBase<PCExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 187,
            EQUIPMENT_PURPOSE = 385;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_PC = 929,
                             HMI_MANUFACTURER = 884,
                             HMI_SOFTWARE = 1949,
                             NETWORK_SCHEME = 868,
                             OPERATING_SYSTEMS = 997,
                             OWNED_BY = 1947,
                             PC_TYP = 1122,
                             RAID = 1198,
                             RAM_MEMORY = 1948,
                             SOFTWARE_LICENSE_NUM = 1950,
                             NARUC_MAINTENANCE_ACCOUNT = 2139,
                             NARUC_OPERATIONS_ACCOUNT = 2140,
                             STANDBY_POWER_TP = 1371;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string HMISoftware { get; set; }
        public string NetworkScheme { get; set; }
        public string OperatingSystem { get; set; }
        public string OwnedBy { get; set; }
        public string PCType { get; set; }
        public string RAID { get; set; }
        public string RAMMemory { get; set; }
        public string SoftwareLicense { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2404;
        protected override int NARUCSpecialMtnNoteDetailsId => 2405;

        protected override string EquipmentType => "PC";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_PC),
                mapper.String(HMISoftware, nameof(HMISoftware), Characteristics.HMI_SOFTWARE),
                mapper.DropDown(NetworkScheme, nameof(NetworkScheme), Characteristics.NETWORK_SCHEME),
                mapper.DropDown(OperatingSystem, nameof(OperatingSystem), Characteristics.OPERATING_SYSTEMS),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PCType, nameof(PCType), Characteristics.PC_TYP),
                mapper.DropDown(RAID, nameof(RAID), Characteristics.RAID),
                mapper.String(RAMMemory, nameof(RAMMemory), Characteristics.RAM_MEMORY),
                mapper.String(SoftwareLicense, nameof(SoftwareLicense), Characteristics.SOFTWARE_LICENSE_NUM),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}