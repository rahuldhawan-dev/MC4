using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class GrinderExcelRecord : EquipmentExcelRecordBase<GrinderExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 165,
                         EQUIPMENT_PURPOSE = 361;

        public struct Characteristics
        {
            public const int EAM_PIPE_SIZE = 1151,
                             GRINDER_TYP = 1477,
                             MOUNTING_GRINDER = 1364,
                             NARUC_MAINTENANCE_ACCOUNT = 2096,
                             NARUC_OPERATIONS_ACCOUNT = 2097,
                             NUMBER_OF_CUTTER_TEETH = 1196,
                             ORIENTATION = 1362;
        }

        #endregion

        #region Properties

        public string GrinderType { get; set; }
        public string Mounting { get; set; }
        public string NumberofCutterTee { get; set; }
        public string Orientation { get; set; }
        public string OwnedBy { get; set; }
        public string PipeChannelSize { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAccount { get; set; }
        public string NARUCOperationsAccount { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "GRINDER";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2362;
        protected override int NARUCSpecialMtnNoteDetailsId => 2363;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(GrinderType, nameof(GrinderType), Characteristics.GRINDER_TYP),
                mapper.DropDown(Mounting, nameof(Mounting), Characteristics.MOUNTING_GRINDER),
                mapper.DropDown(NumberofCutterTee, nameof(NumberofCutterTee), Characteristics.NUMBER_OF_CUTTER_TEETH),
                mapper.DropDown(Orientation, nameof(Orientation), Characteristics.ORIENTATION),
                mapper.String(NARUCMaintenanceAccount, nameof(NARUCMaintenanceAccount), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAccount, nameof(NARUCOperationsAccount), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}