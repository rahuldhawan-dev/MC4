using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class WellExcelRecord : EquipmentExcelRecordBase<WellExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 237, EQUIPMENT_PURPOSE = 431;

        public struct Characteristics
        {
            #region Constants

            public const int DEPTH_IN_FT = 1805,
                             DIAMETER_BOTTOM_IN = 1808,
                             DIAMETER_TOP_IN = 1807,
                             NARUC_MAINTENANCE_ACCOUNT = 2257,
                             NARUC_OPERATIONS_ACCOUNT = 2258,
                             OWNED_BY = 1801,
                             PERMITNUM = 1802,
                             PERMIT_DURATION_YRS = 1804,
                             SPECIAL_MAINT_NOTES = 1809,
                             STATIC_WATER_LEVEL = 1806,
                             WELL_CAPACITY_RATING = 864,
                             WELL_TYP = 1033;

            #endregion
        }

        #endregion

        #region Properties

        public string CapacityGPM { get; set; }
        public string DepthinFT { get; set; }
        public string DiameterBottomin { get; set; }
        public string DiameterTopin { get; set; }
        public string OwnedBy { get; set; }
        public string PermitDurationYrs { get; set; }
        public string PermitLastRenewal { get; set; }
        public string Permit { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string StaticWaterLevel { get; set; }
        public string WellType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;

        protected override int NARUCSpecialMtnNotesId => 2503;
        protected override int NARUCSpecialMtnNoteDetailsId => 2504;

        protected override string EquipmentType => "WELL";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(CapacityGPM, nameof(CapacityGPM), Characteristics.WELL_CAPACITY_RATING),
                mapper.String(DepthinFT, nameof(DepthinFT), Characteristics.DEPTH_IN_FT),
                mapper.String(DiameterBottomin, nameof(DiameterBottomin), Characteristics.DIAMETER_BOTTOM_IN),
                mapper.String(DiameterTopin, nameof(DiameterTopin), Characteristics.DIAMETER_TOP_IN),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(PermitDurationYrs, nameof(PermitDurationYrs), Characteristics.PERMIT_DURATION_YRS),
                mapper.String(Permit, nameof(Permit), Characteristics.PERMITNUM),
                mapper.String(StaticWaterLevel, nameof(StaticWaterLevel), Characteristics.STATIC_WATER_LEVEL),
                mapper.DropDown(WellType, nameof(WellType), Characteristics.WELL_TYP),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}
