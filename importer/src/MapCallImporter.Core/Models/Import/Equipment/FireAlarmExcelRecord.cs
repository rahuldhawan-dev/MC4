using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class FireAlarmExcelRecord : EquipmentExcelRecordBase<FireAlarmExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 157, EQUIPMENT_PURPOSE = 350;

        public struct Characteristics
        {
            #region Constants

            public const int ACTION_TAKEN_UPON_ALARM = 1024,
                             FIRE_CLASS_RATING = 1009,
                             FIRE_AL_TYP = 985,
                             OWNED_BY = 1904,
                             NARUC_MAINTENANCE_ACCOUNT = 2080,
                             NARUC_OPERATIONS_ACCOUNT = 2081,
                             RETEST_REQUIRED = 967;

            #endregion
        }

        #endregion

        #region Properties

        public string ActionTakenUponAl { get; set; }
        public string FireAlarmType { get; set; }
        public string FireClassRating { get; set; }
        public string OwnedBy { get; set; }
        public string RetestRequired { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2346;
        protected override int NARUCSpecialMtnNoteDetailsId => 2347;

        protected override string EquipmentType => "FIRE ALARM";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(ActionTakenUponAl, nameof(ActionTakenUponAl), Characteristics.ACTION_TAKEN_UPON_ALARM),
                mapper.DropDown(FireAlarmType, nameof(FireAlarmType), Characteristics.FIRE_AL_TYP),
                mapper.DropDown(FireClassRating, nameof(FireClassRating), Characteristics.FIRE_CLASS_RATING),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}