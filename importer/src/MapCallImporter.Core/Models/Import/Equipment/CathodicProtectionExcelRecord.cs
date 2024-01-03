using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class CathodicProtectionExcelRecord : EquipmentExcelRecordBase<CathodicProtectionExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 129,
                         EQUIPMENT_PURPOSE = 324;

        public struct Characteristics
        {
            public const int APPLICATION_CATHODIC = 1537,
                             CATHODIC_TYP = 962,
                             NARUC_MAINTENANCE_ACCOUNT = 2024,
                             NARUC_OPERATIONS_ACCOUNT = 2025,
                             VOLT_RATING_CATHODIC = 934;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string CathodicProtection { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "CATHODIC PROTECTION";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2290;
        protected override int NARUCSpecialMtnNoteDetailsId => 2291;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_CATHODIC),
                mapper.DropDown(CathodicProtection, nameof(CathodicProtection), Characteristics.CATHODIC_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING_CATHODIC),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}