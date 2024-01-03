using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class NetworkSwitchExcelRecord : EquipmentExcelRecordBase<NetworkSwitchExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 143,
                         EQUIPMENT_PURPOSE = 382;

        public struct Characteristics
        {
            public const int ANNUAL_COST = 824,
                             APPLICATION_COMM_SW = 1048,
                             BAUD_RATE = 1491,
                             COMM_SW_TYP = 900,
                             COMMUNICATION_TP = 1156,
                             NARUC_MAINTENANCE_ACCOUNT = 2052,
                             NARUC_OPERATIONS_ACCOUNT = 2053,
                             OWNED_BY = 1883,
                             STANDBY_POWER_TP = 1276;
        }

        #endregion

        #region Properties

        public string AnnualFee { get; set; }
        public string Application { get; set; }
        public string BaudRate { get; set; }
        public string CommunicationSwitch { get; set; }
        public string CommunicationType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "NETWORK SWITCH";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2318;
        protected override int NARUCSpecialMtnNoteDetailsId => 2319;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Currency(AnnualFee, nameof(AnnualFee), Characteristics.ANNUAL_COST),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMM_SW),
                mapper.DropDown(BaudRate, nameof(BaudRate), Characteristics.BAUD_RATE),
                mapper.DropDown(CommunicationSwitch, nameof(CommunicationSwitch), Characteristics.COMM_SW_TYP),
                mapper.DropDown(CommunicationType, nameof(CommunicationType), Characteristics.COMMUNICATION_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}