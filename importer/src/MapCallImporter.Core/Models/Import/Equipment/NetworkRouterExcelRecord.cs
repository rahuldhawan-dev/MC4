using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class NetworkRouterExcelRecord : EquipmentExcelRecordBase<NetworkRouterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 142,
                         EQUIPMENT_PURPOSE = 381;

        public struct Characteristics
        {
            public const int ANNUAL_COST = 823,
                             APPLICATION_COMM_RTR = 1534,
                             BAUD_RATE = 947,
                             COMM_RTR_TYP = 1266,
                             COMMUNICATION_TP = 1121,
                             NARUC_MAINTENANCE_ACCOUNT = 2050,
                             NARUC_OPERATIONS_ACCOUNT = 2051,
                             OWNED_BY = 1882,
                             STANDBY_POWER_TP = 1290;
        }

        #endregion

        #region Properties

        public string AnnualFee { get; set; }
        public string Application { get; set; }
        public string BaudRate { get; set; }
        public string CommunicationRouter { get; set; }
        public string CommunicationType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "NETWORK ROUTER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2316;
        protected override int NARUCSpecialMtnNoteDetailsId => 2317;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Currency(AnnualFee, nameof(AnnualFee), Characteristics.ANNUAL_COST),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMM_RTR),
                mapper.DropDown(BaudRate, nameof(BaudRate), Characteristics.BAUD_RATE),
                mapper.DropDown(CommunicationRouter, nameof(CommunicationRouter), Characteristics.COMM_RTR_TYP),
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