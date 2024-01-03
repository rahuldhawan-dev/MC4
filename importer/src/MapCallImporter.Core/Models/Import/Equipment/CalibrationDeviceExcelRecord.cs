using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class CalibrationDeviceExcelRecord : EquipmentExcelRecordBase<CalibrationDeviceExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 128,
                         EQUIPMENT_PURPOSE = 323;

        public struct Characteristics
        {
            public const int CALIB_TYP = 1439,
                             NARUC_MAINTENANCE_ACCOUNT = 2022,
                             NARUC_OPERATIONS_ACCOUNT = 2023,
                             OWNED_BY = 1713,
                             RETEST_REQUIRED = 880,
                             SPECIAL_MAINT_NOTES = 1714;
        }

        #endregion

        #region Properties

        public string CalibratorType { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CALIBRATION DEVICE";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2288;
        protected override int NARUCSpecialMtnNoteDetailsId => 2289;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(CalibratorType, nameof(CalibratorType), Characteristics.CALIB_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
            };
        }

        #endregion
    }
}