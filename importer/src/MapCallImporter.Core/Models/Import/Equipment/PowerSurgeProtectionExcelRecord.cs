using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerSurgeProtectionExcelRecord : EquipmentExcelRecordBase<PowerSurgeProtectionExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 207, EQUIPMENT_PURPOSE = 397;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 810,
                             OWNED_BY = 1968,
                             PWRSURG_TYP = 1109,
                             SYSTEM_PROTECTED = 1002,
                             NARUC_MAINTENANCE_ACCOUNT = 2183,
                             NARUC_OPERATIONS_ACCOUNT = 2184,
                             VOLT_RATING = 989;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string OwnedBy { get; set; }
        public string PowerSurgeType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string SystemProtected { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => 207;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2444;
        protected override int NARUCSpecialMtnNoteDetailsId => 2445;

        protected override string EquipmentType => "POWER SURGE PROTECTION";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerSurgeType, nameof(PowerSurgeType), Characteristics.PWRSURG_TYP),
                mapper.DropDown(SystemProtected, nameof(SystemProtected), Characteristics.SYSTEM_PROTECTED),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}