using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ServerExcelRecord : EquipmentExcelRecordBase<ServerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 217,
                         EQUIPMENT_PURPOSE = 415;

        public struct Characteristics
        {
            public const int APPLICATION_SERVR = 1581,
                             HMI_MANUFACTURER = 1031,
                             HMI_SOFTWARE = 1981,
                             NARUC_MAINTENANCE_ACCOUNT = 2207,
                             NARUC_OPERATIONS_ACCOUNT = 2208,
                             NETWORK_SCHEME = 1574,
                             OPERATING_SYSTEMS = 882,
                             OWNED_BY = 1979,
                             RAID = 1322,
                             RAM_MEMORY = 1980,
                             SERVR_TYP = 1006,
                             SOFTWARE_LICENSE = 1982,
                             STANDBY_POWER_TP = 1189;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string HMISoftware { get; set; }
        public string NetworkScheme { get; set; }
        public string OperatingSystem { get; set; }
        public string OwnedBy { get; set; }
        public string RAID { get; set; }
        public string RAMMemory { get; set; }
        public string ServerType { get; set; }
        public string SoftwareLicense { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "SERVER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2463;
        protected override int NARUCSpecialMtnNoteDetailsId => 2464;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_SERVR),
                mapper.String(HMISoftware, nameof(HMISoftware), Characteristics.HMI_SOFTWARE),
                mapper.DropDown(NetworkScheme, nameof(NetworkScheme), Characteristics.NETWORK_SCHEME),
                mapper.DropDown(OperatingSystem, nameof(OperatingSystem), Characteristics.OPERATING_SYSTEMS),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RAID, nameof(RAID), Characteristics.RAID),
                mapper.String(RAMMemory, nameof(RAMMemory), Characteristics.RAM_MEMORY),
                mapper.DropDown(ServerType, nameof(ServerType), Characteristics.SERVR_TYP),
                mapper.String(SoftwareLicense, nameof(SoftwareLicense), Characteristics.SOFTWARE_LICENSE),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}