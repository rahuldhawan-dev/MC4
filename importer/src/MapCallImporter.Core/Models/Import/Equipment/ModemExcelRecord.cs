using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ModemExcelRecord : EquipmentExcelRecordBase<ModemExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 140,
                         EQUIPMENT_PURPOSE = 377;

        public struct Characteristics
        {
            public const int ANNUAL_COST = 826,
                             APPLICATION_COMM_MOD = 1296,
                             BAUD_RATE = 1298,
                             COMM_MOD_TYP = 1060,
                             COMMUNICATION_TP = 1214,
                             NARUC_MAINTENANCE_ACCOUNT = 2046,
                             NARUC_OPERATIONS_ACCOUNT = 2047,
                             OWNED_BY = 1881,
                             STANDBY_POWER_TP = 1182;
        }

        #endregion

        #region Properties

        public string AnnualFee { get; set; }
        public string Application { get; set; }
        public string BaudRate { get; set; }
        public string CommunicationModem { get; set; }
        public string CommunicationType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "MODEM";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2312;
        protected override int NARUCSpecialMtnNoteDetailsId => 2313;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Currency(AnnualFee, nameof(AnnualFee), Characteristics.ANNUAL_COST),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMM_MOD),
                mapper.DropDown(BaudRate, nameof(BaudRate), Characteristics.BAUD_RATE),
                mapper.DropDown(CommunicationModem, nameof(CommunicationModem), Characteristics.COMM_MOD_TYP),
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