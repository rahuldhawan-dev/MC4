using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class TelephoneExcelRecord : EquipmentExcelRecordBase<TelephoneExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 144,
                         EQUIPMENT_PURPOSE = 418;

        public struct Characteristics
        {
            public const int ANNUAL_COST = 821,
                             ANTENNA_TP = 1375,
                             APPLICATION_COMM_TEL = 1273,
                             BAUD_RATE = 1517,
                             COMM_TEL_TYP = 1013,
                             COMMUNICATION_TP = 1164,
                             NARUC_MAINTENANCE_ACCOUNT = 2054,
                             NARUC_OPERATIONS_ACCOUNT = 2055,
                             OWNED_BY = 1884,
                             POWER_WATTS = 1885,
                             STANDBY_POWER_TP = 1244;
        }

        #endregion

        #region Properties

        public string AnnualFee { get; set; }
        public string AntennaType { get; set; }
        public string Application { get; set; }
        public string BaudRate { get; set; }
        public string CommunicationTeleph { get; set; }
        public string CommunicationType { get; set; }
        public string OwnedBy { get; set; }
        public string Powerwatts { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "TELEPHONE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2320;
        protected override int NARUCSpecialMtnNoteDetailsId => 2321;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Currency(AnnualFee, AnnualFee, Characteristics.ANNUAL_COST),
                mapper.DropDown(AntennaType, nameof(AntennaType), Characteristics.ANTENNA_TP),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMM_TEL),
                mapper.DropDown(BaudRate, nameof(BaudRate), Characteristics.BAUD_RATE),
                mapper.DropDown(CommunicationTeleph, nameof(CommunicationTeleph), Characteristics.COMM_TEL_TYP),
                mapper.DropDown(CommunicationType, nameof(CommunicationType), Characteristics.COMMUNICATION_TP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(Powerwatts, nameof(Powerwatts), Characteristics.POWER_WATTS),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}