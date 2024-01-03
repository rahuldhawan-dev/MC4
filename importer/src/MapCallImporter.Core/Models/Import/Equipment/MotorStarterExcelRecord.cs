using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class MotorStarterExcelRecord : EquipmentExcelRecordBase<MotorStarterExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 184, EQUIPMENT_PURPOSE = 380;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 817,
                             FUSE_SIZE = 1740,
                             HP_RATING = 850,
                             MOTSTR_TYP = 1015,
                             OWNED_BY = 1739,
                             SPECIAL_MAINT_NOTES = 1741,
                             STR_NEMA_SIZE = 1378,
                             STR_OVERLOAD_TP = 982,
                             STR_TP = 959,
                             NARUC_MAINTENANCE_ACCOUNT = 2134,
                             NARUC_OPERATIONS_ACCOUNT = 2135,
                             VOLT_RATING = 1219;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string FuseSize { get; set; }
        public string HPRating { get; set; }
        public string MotorStarterType { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StrNEMASize { get; set; }
        public string StrOverloadType { get; set; }
        public string StrType { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2400;
        protected override int NARUCSpecialMtnNoteDetailsId => 2401;

        protected override string EquipmentType => "MOTOR STARTER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(FuseSize, nameof(FuseSize), Characteristics.FUSE_SIZE),
                mapper.Numerical(HPRating, nameof(HPRating), Characteristics.HP_RATING),
                mapper.DropDown(MotorStarterType, nameof(MotorStarterType), Characteristics.MOTSTR_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(StrNEMASize, nameof(StrNEMASize), Characteristics.STR_NEMA_SIZE),
                mapper.DropDown(StrOverloadType, nameof(StrOverloadType), Characteristics.STR_OVERLOAD_TP),
                mapper.DropDown(StrType, nameof(StrType), Characteristics.STR_TP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
