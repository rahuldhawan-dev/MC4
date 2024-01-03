using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerDisconnectExcelRecord : EquipmentExcelRecordBase<PowerDisconnectExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 202, EQUIPMENT_PURPOSE = 392;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_CURRRATING = 796,
                             AMP_RATING = 801,
                             FUSE_SIZE = 1959,
                             OWNED_BY = 1958,
                             PWRDISC_TYP = 892,
                             NARUC_MAINTENANCE_ACCOUNT = 2173,
                             NARUC_OPERATIONS_ACCOUNT = 2174,
                             VOLT_RATING = 1251;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string CurrentRating { get; set; }
        public string FuseSize { get; set; }
        public string OwnedBy { get; set; }
        public string PowerDisconnectTyp { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2434;
        protected override int NARUCSpecialMtnNoteDetailsId => 2435;

        protected override string EquipmentType => "POWER DISCONNECT";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.Numerical(CurrentRating, nameof(CurrentRating), Characteristics.AMP_CURRRATING),
                mapper.String(FuseSize, nameof(FuseSize), Characteristics.FUSE_SIZE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerDisconnectTyp, nameof(PowerDisconnectTyp), Characteristics.PWRDISC_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}