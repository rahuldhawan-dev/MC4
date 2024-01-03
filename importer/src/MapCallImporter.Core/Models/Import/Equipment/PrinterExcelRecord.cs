using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PrinterExcelRecord : EquipmentExcelRecordBase<PrinterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 198,
                         EQUIPMENT_PURPOSE = 400;

        public struct Characteristics
        {
            public const int APPLICATION_PRNTR = 1394,
                             NARUC_MAINTENANCE_ACCOUNT = 2165,
                             NARUC_OPERATIONS_ACCOUNT = 2166,
                             NETWORK_SCHEME = 1425,
                             OPERATING_SYSTEMS = 1487,
                             OWNED_BY = 1951,
                             PRNTR_TYP = 1028,
                             RAID = 1582,
                             RAM_MEMORY = 1952,
                             SOFTWARE_LICENSE = 1953,
                             STANDBY_POWER_TP = 1234;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string NetworkScheme { get; set; }
        public string OperatingSystem { get; set; }
        public string OwnedBy { get; set; }
        public string PrinterType { get; set; }
        public string RAID { get; set; }
        public string RAMMemory { get; set; }
        public string SoftwareLicense { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StandbyPowerType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "PRINTER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2426;
        protected override int NARUCSpecialMtnNoteDetailsId => 2427;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_PRNTR),
                mapper.DropDown(NetworkScheme, nameof(NetworkScheme), Characteristics.NETWORK_SCHEME),
                mapper.DropDown(OperatingSystem, nameof(OperatingSystem), Characteristics.OPERATING_SYSTEMS),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PrinterType, nameof(PrinterType), Characteristics.PRNTR_TYP),
                mapper.DropDown(RAID, nameof(RAID), Characteristics.RAID),
                mapper.String(RAMMemory, nameof(RAMMemory), Characteristics.RAM_MEMORY),
                mapper.String(SoftwareLicense, nameof(SoftwareLicense), Characteristics.SOFTWARE_LICENSE),
                mapper.DropDown(StandbyPowerType, nameof(StandbyPowerType), Characteristics.STANDBY_POWER_TP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}