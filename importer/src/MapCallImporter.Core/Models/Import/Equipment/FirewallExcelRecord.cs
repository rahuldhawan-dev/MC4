using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FirewallExcelRecord : EquipmentExcelRecordBase<FirewallExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 139,
                         EQUIPMENT_PURPOSE = 353;

        public struct Characteristics
        {
            public const int ANNUAL_COST = 825,
                             APPLICATION_COMM_FWL = 1352,
                             BAUD_RATE = 1246,
                             COMM_FWL_TYP = 1384,
                             COMMUNICATION_TP = 888,
                             NARUC_MAINTENANCE_ACCOUNT = 2044,
                             NARUC_OPERATIONS_ACCOUNT = 2045,
                             OWNED_BY = 1880,
                             STANDBY_POWER_TP = 1455;
        }

        #endregion

        #region Properties

        public string AnnualFee { get; set; }
        public string Application { get; set; }
        public string BaudRate { get; set; }
        public string CommunicationFirewa { get; set; }
        public string CommunicationType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "FIREWALL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2310;
        protected override int NARUCSpecialMtnNoteDetailsId => 2311;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Currency(AnnualFee, nameof(AnnualFee), Characteristics.ANNUAL_COST),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMM_FWL),
                mapper.DropDown(BaudRate, nameof(BaudRate), Characteristics.BAUD_RATE),
                mapper.DropDown(CommunicationFirewa, nameof(CommunicationFirewa), Characteristics.COMM_FWL_TYP),
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