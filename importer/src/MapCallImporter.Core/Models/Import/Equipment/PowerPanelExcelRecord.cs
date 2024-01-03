using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PowerPanelExcelRecord : EquipmentExcelRecordBase<PowerPanelExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 205,
                         EQUIPMENT_PURPOSE = 395;

        public struct Characteristics
        {
            public const int AMP_RATING = 800,
                             FUSE_SIZE = 1965,
                             FUSED = 874,
                             NARUC_MAINTENANCE_ACCOUNT = 2179,
                             NARUC_OPERATIONS_ACCOUNT = 2180,
                             NUMBER_OF_BREAKERS = 860,
                             OWNED_BY = 1964,
                             PWRPNL_TYP = 1442,
                             VOLT_RATING = 1211;
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string FuseSize { get; set; }
        public string Fused { get; set; }
        public string NumberofBreakers { get; set; }
        public string OwnedBy { get; set; }
        public string PowerPanelType { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "POWER PANEL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2440;
        protected override int NARUCSpecialMtnNoteDetailsId => 2441;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(FuseSize, nameof(FuseSize), Characteristics.FUSE_SIZE),
                mapper.DropDown(Fused, nameof(Fused), Characteristics.FUSED),
                mapper.Numerical(NumberofBreakers, nameof(NumberofBreakers), Characteristics.NUMBER_OF_BREAKERS),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(PowerPanelType, nameof(PowerPanelType), Characteristics.PWRPNL_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}