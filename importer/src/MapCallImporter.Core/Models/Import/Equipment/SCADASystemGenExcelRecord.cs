using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class SCADASystemGenExcelRecord : EquipmentExcelRecordBase<SCADASystemGenExcelRecord>
    {
        #region Constants

        public struct Characteristics
        {
            #region Constants

            public const int OWNED_BY = 1973,
                             SCADA_HMI_AND_DATABASE = 1972,
                             SCADASYS_TYP = 1230,
                             SPECIAL_MAINT_NOTES = 2196,
                             NARUC_MAINTENANCE_ACCOUNT = 2197,
                             NARUC_OPERATIONS_ACCOUNT = 2198;

            #endregion
        }

        // SCADA SYSTEM GEN
        public const int EQUIPMENT_TYPE = 212, EQUIPMENT_PURPOSE = 409;

        #endregion

        #region Properties

        public string OwnedBy { get; set; }
        public string SCADAHMIandDataba { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2453;
        protected override int NARUCSpecialMtnNoteDetailsId => 2454;

        protected override string EquipmentType => "SCADA SYSTEM GEN";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(SCADAHMIandDataba, nameof(SCADAHMIandDataba), Characteristics.SCADA_HMI_AND_DATABASE),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}