using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class DamExcelRecord : EquipmentExcelRecordBase<DamExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 149,
                         EQUIPMENT_PURPOSE = 338;

        public struct Characteristics
        {
            public const int DAM_TYP = 1380,
                             HEIGHT_FT = 1897,
                             NARUC_MAINTENANCE_ACCOUNT = 2064,
                             NARUC_OPERATIONS_ACCOUNT = 2065,
                             NORMAL_POOL_CAPACITY_MG = 1899,
                             NORMAL_POOL_HEIGHT_FT = 1898,
                             OWNED_BY = 1895,
                             STATE_ID_NUMBER = 1896;
        }

        #endregion

        #region Properties

        public string DamType { get; set; }
        public string HeightFT { get; set; }
        public string NormalPoolCapacity { get; set; }
        public string NormalPoolHeight { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StateIDNumber { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "DAM";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2330;
        protected override int NARUCSpecialMtnNoteDetailsId => 2331;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(DamType, nameof(DamType), Characteristics.DAM_TYP),
                mapper.String(HeightFT, nameof(HeightFT), Characteristics.HEIGHT_FT),
                mapper.String(NormalPoolCapacity, nameof(NormalPoolCapacity), Characteristics.NORMAL_POOL_CAPACITY_MG),
                mapper.String(NormalPoolHeight, nameof(NormalPoolHeight), Characteristics.NORMAL_POOL_HEIGHT_FT),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(StateIDNumber, nameof(StateIDNumber), Characteristics.STATE_ID_NUMBER),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}