using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FacilityAndGroundsExcelRecord : EquipmentExcelRecordBase<FacilityAndGroundsExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 156, EQUIPMENT_PURPOSE = 347;

        public struct Characteristics
        {
            public const int BACKUP_POWER = 1395,
                             BLOCK = 1679,
                             FACILITY_TYP = 1399,
                             FACILITY_FIRE_ALARM = 1100,
                             LOT = 1680,
                             ON_SCADA = 914,
                             OWNED_BY = 1902,
                             FACILITY_SECURITY_TP = 1181,
                             FACILITY_STAFFING = 1390,
                             TOTAL_SQ_FT = 1903,
                             NARUC_MAINTENANCE_ACCOUNT = 2078,
                             NARUC_OPERATIONS_ACCOUNT = 2079,
                             FACILITY_VOLTAGE = 1201;
        }

        #endregion

        #region Properties

        public string BackupPower { get; set; }
        public string Block { get; set; }
        public string FacilityType { get; set; }
        public string FireAlarm { get; set; }
        public string Lot { get; set; }
        public string OnSCADA { get; set; }
        public string OwnedBy { get; set; }
        public string SecurityType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string Staffing { get; set; }
        public string TotalSqft { get; set; }
        public string VoltageEntering { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2344;
        protected override int NARUCSpecialMtnNoteDetailsId => 2345;

        protected override string EquipmentType => "FACILITY AND GROUNDS";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(BackupPower, nameof(BackupPower), Characteristics.BACKUP_POWER),
                mapper.Numerical(Block, nameof(Block), Characteristics.BLOCK),
                mapper.DropDown(FacilityType, nameof(FacilityType), Characteristics.FACILITY_TYP),
                mapper.DropDown(FireAlarm, nameof(FireAlarm), Characteristics.FACILITY_FIRE_ALARM),
                mapper.Numerical(Lot, nameof(Lot), Characteristics.LOT),
                mapper.DropDown(OnSCADA, nameof(OnSCADA), Characteristics.ON_SCADA),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(SecurityType, nameof(SecurityType), Characteristics.FACILITY_SECURITY_TP),
                mapper.DropDown(Staffing, nameof(Staffing), Characteristics.FACILITY_STAFFING),
                mapper.String(TotalSqft, nameof(TotalSqft), Characteristics.TOTAL_SQ_FT),
                mapper.DropDown(VoltageEntering, nameof(VoltageEntering), Characteristics.FACILITY_VOLTAGE),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}