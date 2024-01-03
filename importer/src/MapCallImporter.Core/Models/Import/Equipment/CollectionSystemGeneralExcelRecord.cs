using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class CollectionSystemGeneralExcelRecord : EquipmentExcelRecordBase<CollectionSystemGeneralExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 138,
                         EQUIPMENT_PURPOSE = 334;

        public struct Characteristics
        {
            public const int COLLSYS_TYP = 935,
                             NARUC_MAINTENANCE_ACCOUNT = 2042,
                             NARUC_OPERATIONS_ACCOUNT = 2043,
                             OWNED_BY = 1879;
        }

        #endregion

        #region Properties

        public string CollectionSystemType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "COLLECTION SYSTEM GENERAL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2308;
        protected override int NARUCSpecialMtnNoteDetailsId => 2309;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(CollectionSystemType, nameof(CollectionSystemType), Characteristics.COLLSYS_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}