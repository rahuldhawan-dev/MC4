using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerConditionerExcelRecord : EquipmentExcelRecordBase<PowerConditionerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 201,
                         EQUIPMENT_PURPOSE = 391;

        public struct Characteristics
        {
            public const int ADJUSTABLE = 1199,
                             AMP_RATING = 798,
                             NARUC_MAINTENANCE_ACCOUNT = 2171,
                             NARUC_OPERATIONS_ACCOUNT = 2172,
                             OWNED_BY = 1957,
                             PWRCOND_TYP = 1335,
                             VOLT_RATING = 1389;
        }

        #endregion

        #region Properties

        public string Adjustable { get; set; }
        public string AmpRating { get; set; }
        public string OwnedBy { get; set; }
        public string PowerConditionerTy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "POWER CONDITIONER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2432;
        protected override int NARUCSpecialMtnNoteDetailsId => 2433;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Adjustable, nameof(Adjustable), Characteristics.ADJUSTABLE),
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerConditionerTy, nameof(PowerConditionerTy), Characteristics.PWRCOND_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}