using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class AirCompressorExcelRecord : EquipmentExcelRecordBase<AirCompressorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 145, EQUIPMENT_PURPOSE = 312;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_COMP = 920,
                             BHP_RATING = 829,
                             CAPACITY_RATING = 1726,
                             CAPACITY_UOM_COMP = 836,
                             COMP_TYP = 1083,
                             DRIVE_TP = 1415,
                             MAX_PRESSURE = 1728,
                             OWNED_BY = 1724,
                             RPM_OPERATING = 1725,
                             SPECIAL_MAINT_NOTES = 1729,
                             STAGES = 1187,
                             NARUC_MAINTENANCE_ACCOUNT = 2056,
                             NARUC_OPERATIONS_ACCOUNT = 2057,
                             TNK_VOL_GAL = 1727;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BHPRating { get; set; }
        public string CapacityRating { get; set; }
        public string CapacityUOM { get; set; }
        public string CompressorType { get; set; }
        public string DriveType { get; set; }
        public string MaxPressure { get; set; }
        public string OwnedBy { get; set; }
        public string RPMOperating { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string Stages { get; set; }
        public string TnkVolumegal { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2322;
        protected override int NARUCSpecialMtnNoteDetailsId => 2323;

        protected override string EquipmentType => "AIR COMPRESSOR";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_COMP),
                mapper.Numerical(BHPRating, nameof(BHPRating), Characteristics.BHP_RATING),
                mapper.String(CapacityRating, nameof(CapacityRating), Characteristics.CAPACITY_RATING),
                mapper.String(CapacityUOM, nameof(CapacityUOM), Characteristics.CAPACITY_UOM_COMP),
                mapper.DropDown(CompressorType, nameof(CompressorType), Characteristics.COMP_TYP),
                mapper.DropDown(DriveType, nameof(DriveType), Characteristics.DRIVE_TP),
                mapper.String(MaxPressure, nameof(MaxPressure), Characteristics.MAX_PRESSURE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(RPMOperating, nameof(RPMOperating), Characteristics.RPM_OPERATING),
                mapper.DropDown(Stages, nameof(Stages), Characteristics.STAGES),
                mapper.String(TnkVolumegal, nameof(TnkVolumegal), Characteristics.TNK_VOL_GAL),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
