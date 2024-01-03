using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ScreenExcelRecord : EquipmentExcelRecordBase<ScreenExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 215,
                         EQUIPMENT_PURPOSE = 411;

        public struct Characteristics
        {
            public const int APPLICATION_SCREEN = 1527,
                             AUTO_WASH = 878,
                             LOCATION = 897,
                             NARUC_MAINTENANCE_ACCOUNT = 2203,
                             NARUC_OPERATIONS_ACCOUNT = 2204,
                             OWNED_BY = 1976,
                             RPM_OPERATING = 1977,
                             SCREEN_TYP = 1171;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string AutoWash { get; set; }
        public string IndoorOutdoor { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string ScreenType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "SCREEN";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2459;
        protected override int NARUCSpecialMtnNoteDetailsId => 2460;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_SCREEN),
                mapper.DropDown(AutoWash, nameof(AutoWash), Characteristics.AUTO_WASH),
                mapper.DropDown(IndoorOutdoor, nameof(IndoorOutdoor), Characteristics.LOCATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.DropDown(ScreenType, nameof(ScreenType), Characteristics.SCREEN_TYP),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}