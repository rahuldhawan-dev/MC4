using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ConveyorExcelRecord : EquipmentExcelRecordBase<ConveyorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 148,
                         EQUIPMENT_PURPOSE = 336;

        public struct Characteristics
        {
            public const int CONVEYOR_TYP = 1077,
                             LENGTH_FT = 1892,
                             LIFT_FT = 1894,
                             NARUC_MAINTENANCE_ACCOUNT = 2062,
                             NARUC_OPERATIONS_ACCOUNT = 2063,
                             OWNED_BY = 1889,
                             RPM_OPERATING = 1891,
                             SPEED_FPS = 1890,
                             WIDTH_FT = 1893;
        }

        #endregion

        #region Properties

        public string ConveyorType { get; set; }
        public string LengthFT { get; set; }
        public string LiftFT { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SpeedFPS { get; set; }
        public string WidthFT { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CONVEYOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2328;
        protected override int NARUCSpecialMtnNoteDetailsId => 2329;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(ConveyorType, nameof(ConveyorType), Characteristics.CONVEYOR_TYP),
                mapper.String(LengthFT, nameof(LengthFT), Characteristics.LENGTH_FT),
                mapper.String(LiftFT, nameof(LiftFT), Characteristics.LIFT_FT),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.String(SpeedFPS, nameof(SpeedFPS), Characteristics.SPEED_FPS),
                mapper.String(WidthFT, nameof(WidthFT), Characteristics.WIDTH_FT),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}