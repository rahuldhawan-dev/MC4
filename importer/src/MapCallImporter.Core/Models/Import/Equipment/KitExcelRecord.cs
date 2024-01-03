using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class KitExcelRecord : EquipmentExcelRecordBase<KitExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 178,
                         EQUIPMENT_PURPOSE = 372;

        public struct Characteristics
        {
            public const int KIT_TYP = 1397,
                             NARUC_MAINTENANCE_ACCOUNT = 2122,
                             NARUC_OPERATIONS_ACCOUNT = 2123,
                             OWNED_BY = 1946,
                             RETEST_REQUIRED = 1124;
        }

        #endregion

        #region Properties

        public string KitType { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "KIT (SAFETY, REPAIR, HAZWOPR)";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2388;
        protected override int NARUCSpecialMtnNoteDetailsId => 2389;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(KitType, nameof(KitType), Characteristics.KIT_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}