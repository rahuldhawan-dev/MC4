using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class AeratorExcelRecord : EquipmentExcelRecordBase<AeratorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 228,
                         EQUIPMENT_PURPOSE = 311;

        public struct Characteristics
        {
            public const int DIFFUSER_MATERIAL = 1045,
                             MEMBRANE_MATERIAL = 1168,
                             NARUC_MAINTENANCE_ACCOUNT = 2239,
                             NARUC_OPERATIONS_ACCOUNT = 2240,
                             TRT_AER_TYP = 1391;
        }

        #endregion

        #region Properties

        public string AeratorType { get; set; }
        public string CFM { get; set; }
        public string DiffuserMaterial { get; set; }
        public string MembraneMaterial { get; set; }
        public string OwnedBy { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "AERATOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2485;
        protected override int NARUCSpecialMtnNoteDetailsId => 2486;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(AeratorType, nameof(AeratorType), Characteristics.TRT_AER_TYP),
                mapper.DropDown(DiffuserMaterial, nameof(DiffuserMaterial), Characteristics.DIFFUSER_MATERIAL),
                mapper.DropDown(MembraneMaterial, nameof(MembraneMaterial), Characteristics.MEMBRANE_MATERIAL),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT)
            };
        }

        #endregion
    }
}
