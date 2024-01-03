using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FireSuppressionExcelRecord : EquipmentExcelRecordBase<FireSuppressionExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 159, EQUPMENT_TYPE = 352;

        public struct Characteristics
        {
            #region Constants

            public const int ACTION_TAKEN_UPON_ALARM = 1580,
                             FIRE_CLASS_RATING = 910,
                             FIRE_SUP_TYP = 1448,
                             NARUC_MAINTENANCE_ACCOUNT = 2084,
                             NARUC_OPERATIONS_ACCOUNT = 2085,
                             OWNED_BY = 1906,
                             RETEST_REQUIRED = 1039;

            #endregion
        }

        #endregion

        #region Properties

        public string ActionTakenUponAl { get; set; }
        public string FireClassRating { get; set; }
        public string FireSuppressionTyp { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUPMENT_TYPE;
        protected override int NARUCSpecialMtnNotesId => 2350;
        protected override int NARUCSpecialMtnNoteDetailsId => 2351;

        protected override string EquipmentType => "FIRE SUPPRESSION";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(FireSuppressionTyp, nameof(FireSuppressionTyp), Characteristics.FIRE_SUP_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(FireClassRating, nameof(FireClassRating), Characteristics.FIRE_CLASS_RATING),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.DropDown(ActionTakenUponAl, nameof(ActionTakenUponAl), Characteristics.ACTION_TAKEN_UPON_ALARM),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}