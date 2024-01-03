using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class SCADARadioExcelRecord : EquipmentExcelRecordBase<SCADARadioExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 141, EQUIPMENT_PURPOSE = 408;

        public struct Characteristics
        {
            public const int ANNUAL_COST = 822,
                             ANTENNA_TP = 1351,
                             APPLICATION_COMM_RAD = 1127,
                             BAUD_RATE = 896,
                             COMM_RAD_TYP = 1445,
                             COMMUNICATION_TP = 908,
                             FCC_LICENSE = 1769,
                             OWNED_BY = 1768,
                             NARUC_MAINTENANCE_ACCOUNT = 2048,
                             NARUC_OPERATIONS_ACCOUNT = 2049,
                             POWER_WATTS = 1770,
                             RECEIVE_FREQUENCY = 1772,
                             STANDBY_POWER_TP = 928,
                             TRANSMIT_FREQUENCY = 1771,
                             SPECIAL_MAINT_NOTES = 1773;
        }

        #endregion

        #region Properties

        public string AnnualFee { get; set; }
        public string AntennaType { get; set; }
        public string Application { get; set; }
        public string BaudRate { get; set; }
        public string CommunicationRadio { get; set; }
        public string CommunicationType { get; set; }
        public string FCCLicense { get; set; }
        public string OwnedBy { get; set; }
        public string Powerwatts { get; set; }
        public string ReceiveFrequency { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string TransmitFrequency { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2314;
        protected override int NARUCSpecialMtnNoteDetailsId => 2315;

        protected override string EquipmentType => "SCADA RADIO";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Currency(AnnualFee, nameof(AnnualFee), Characteristics.ANNUAL_COST),
                mapper.DropDown(AntennaType, nameof(AntennaType), Characteristics.ANTENNA_TP),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMM_RAD),
                mapper.DropDown(BaudRate, nameof(BaudRate), Characteristics.BAUD_RATE),
                mapper.DropDown(CommunicationRadio, nameof(CommunicationRadio), Characteristics.COMM_RAD_TYP),
                mapper.DropDown(CommunicationType, nameof(CommunicationType), Characteristics.COMMUNICATION_TP),
                mapper.String(FCCLicense, nameof(FCCLicense), Characteristics.FCC_LICENSE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(Powerwatts, nameof(Powerwatts), Characteristics.POWER_WATTS),
                mapper.String(ReceiveFrequency, nameof(ReceiveFrequency), Characteristics.RECEIVE_FREQUENCY),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(TransmitFrequency, nameof(TransmitFrequency), Characteristics.TRANSMIT_FREQUENCY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
