using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class AMIDATACOLLExcelRecord : EquipmentExcelRecordBase<AMIDATACOLLExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 241,
                         EQUIPMENT_PURPOSE = 315;

        public struct Characteristics
        {
            public const int ADMIDC_ANTENNABOT_ELEV = 2518,
                             ADMIDC_ANTENNATOP_ELEV = 2517,
                             ADMIDC_FREQ = 2515,
                             AMIDC_BACKUP_SOURCE = 2516,
                             AMIDC_BATT_SIZE = 2514,
                             AMIDC_LOC = 2511,
                             AMIDC_LOC_ACCESS = 2512,
                             AMIDC_POWR = 2513,
                             INSTALLATION_WO = 2519,
                             NARUC_SPECIAL_MAINT_NOTE_DETAILS = 2522,
                             NARUC_SPECIAL_MAINT_NOTES = 2521,
                             OWNED_BY = 2520;
        }

        #endregion

        #region Properties

        public string AMIDC_LOC { get; set; }
        public string AMIDC_LOC_ACCESS { get; set; }
        public string AMIDC_POWR { get; set; }
        public string AMIDC_BATT_SIZE { get; set; }
        public string AMIDC_FREQ { get; set; }
        public string AMIDC_BACKUP_SOURCE { get; set; }
        public string AMIDC_ANTENNATOP_ELEV { get; set; }
        public string AMIDC_ANTENNABOT_ELEV { get; set; }
        public string INSTALLATION_WO { get; set; }
        public string OWNED_BY { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "AMIDATACOLL";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2521;
        protected override int NARUCSpecialMtnNoteDetailsId => 2522;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(AMIDC_LOC, nameof(AMIDC_LOC), Characteristics.AMIDC_LOC),
                mapper.String(AMIDC_LOC_ACCESS, nameof(AMIDC_LOC_ACCESS), Characteristics.AMIDC_LOC_ACCESS),
                mapper.String(AMIDC_POWR, nameof(AMIDC_POWR), Characteristics.AMIDC_POWR),
                mapper.String(AMIDC_BATT_SIZE, nameof(AMIDC_BATT_SIZE), Characteristics.AMIDC_BATT_SIZE),
                mapper.String(AMIDC_FREQ, nameof(AMIDC_FREQ), Characteristics.ADMIDC_FREQ),
                mapper.String(AMIDC_BACKUP_SOURCE, nameof(AMIDC_BACKUP_SOURCE), Characteristics.AMIDC_BACKUP_SOURCE),
                mapper.String(AMIDC_ANTENNATOP_ELEV, nameof(AMIDC_ANTENNATOP_ELEV), Characteristics.ADMIDC_ANTENNATOP_ELEV),
                mapper.String(AMIDC_ANTENNABOT_ELEV, nameof(AMIDC_ANTENNABOT_ELEV), Characteristics.ADMIDC_ANTENNABOT_ELEV),
                mapper.String(INSTALLATION_WO, nameof(INSTALLATION_WO), Characteristics.INSTALLATION_WO),
                mapper.String(OWNED_BY, nameof(OWNED_BY), Characteristics.OWNED_BY),
            }; 
        }

        #endregion
    }
}