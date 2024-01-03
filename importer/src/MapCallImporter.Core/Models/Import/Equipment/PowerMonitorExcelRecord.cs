using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerMonitorExcelRecord : EquipmentExcelRecordBase<PowerMonitorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 204, EQUIPMENT_PURPOSE = 394;

        public struct Characteristics
        {
            #region Constants

            public const int ADJUSTABLE = 1523,
                             AMP_RATING = 802,
                             OWNED_BY = 1962,
                             PWRMON_TYP = 1294,
                             SYSTEM_MONITORED = 1963,
                             NARUC_MAINTENANCE_ACCOUNT = 2177,
                             NARUC_OPERATIONS_ACCOUNT = 2178,
                             VOLT_RATING = 1264;

            #endregion
        }

        #endregion

        #region Properties

        public string Adjustable { get; set; }
        public string AmpRating { get; set; }
        public string OwnedBy { get; set; }
        public string PowerMonitorType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SystemMonitored { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2438;
        protected override int NARUCSpecialMtnNoteDetailsId => 2439;

        protected override string EquipmentType => "POWER MONITOR";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Adjustable, nameof(Adjustable), Characteristics.ADJUSTABLE),
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerMonitorType, nameof(PowerMonitorType), Characteristics.PWRMON_TYP),
                mapper.String(SystemMonitored, nameof(SystemMonitored), Characteristics.SYSTEM_MONITORED),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}