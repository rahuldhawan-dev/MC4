using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class RespiratorExcelRecord : EquipmentExcelRecordBase<RespiratorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 196,
                         EQUIPMENT_PURPOSE = 405;

        public struct Characteristics
        {
            public const int GAS_MITIGATED = 1417,
                             NARUC_MAINTENANCE_ACCOUNT = 2159,
                             NARUC_OPERATIONS_ACCOUNT = 2160,
                             OWNED_BY = 1794,
                             PPE_RESP_TYP = 964,
                             RESPIRATOR_RATING = 1443,
                             SPECIAL_MAINT_NOTES = 1795;
        }

        #endregion

        #region Properties

        public string GasDetectedMitiga { get; set; }
        public string OwnedBy { get; set; }
        public string RespiratorRating { get; set; }
        public string RespiratoryProtecti { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "RESPIRATOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2422;
        protected override int NARUCSpecialMtnNoteDetailsId => 2423;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(GasDetectedMitiga, nameof(GasDetectedMitiga), Characteristics.GAS_MITIGATED),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RespiratorRating, nameof(RespiratorRating), Characteristics.RESPIRATOR_RATING),
                mapper.DropDown(RespiratoryProtecti, nameof(RespiratoryProtecti), Characteristics.PPE_RESP_TYP),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}