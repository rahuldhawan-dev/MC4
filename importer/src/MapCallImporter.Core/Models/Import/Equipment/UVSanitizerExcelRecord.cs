using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class UVSanitizerExcelRecord : EquipmentExcelRecordBase<UVSanitizerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 234,
                         EQUIPMENT_PURPOSE = 423;

        public struct Characteristics
        {
            public const int TRT_UV_TYP = 1303;
        }

        #endregion

        #region Properties

        public string NumberofLampsModu { get; set; }
        public string NumberofUVModules { get; set; }
        public string OwnedBy { get; set; }
        public string PeakProcessFlowg { get; set; }
        public string RetentionTimeseco { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string UVSystemType { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "UV SANITIZER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2497;
        protected override int NARUCSpecialMtnNoteDetailsId => 2498;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(UVSystemType, nameof(UVSystemType), Characteristics.TRT_UV_TYP),
            };
        }

        #endregion
    }
}